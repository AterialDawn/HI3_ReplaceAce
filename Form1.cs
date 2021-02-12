using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Principal;

namespace HI3_ReplaceAce
{
    public partial class Form1 : Form
    {
        string audioPck_location;
        bool errored = false;
        public Form1()
        {
            InitializeComponent();
        }

        private void beginButton_Click(object sender, EventArgs e)
        {
            

            bool isElevated;
            using (WindowsIdentity identity = WindowsIdentity.GetCurrent())
            {
                WindowsPrincipal principal = new WindowsPrincipal(identity);
                isElevated = principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
            if (!isElevated)
            {
                MessageBox.Show("This program needs to run as administrator (it requires write access to Program Files)\nRun this program as admin and try again!");
                return;
            }

            using (OpenFileDialog ofd = new OpenFileDialog { Filter = "Honkai Impact 3rd Launcher|falcon_glb.exe;falcon_os.exe|AUDIO_Default.pck|AUDIO_Default.pck", Title = "Find your Honkai 3rd Install folder and select falcon_glb.exe" })
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    if (Path.GetFileName(ofd.FileName) == "AUDIO_Default.pck")
                    {
                        audioPck_location = ofd.FileName;
                    }
                    else
                    {
                        audioPck_location = Path.Combine(Path.GetDirectoryName(ofd.FileName), "Games", "BH3_Data", "StreamingAssets", "Audio", "GeneratedSoundBanks", "Windows", "AUDIO_Default.pck");
                    }
                    if (File.Exists(audioPck_location))
                    {
                        tabControl.SelectedIndex = 1;
                        ReplaceAceWithGion();
                        return;
                    }
                    else
                    {
                        MessageBox.Show("This location does not seem to be correct. Try selecting AUDIO_Default.pck directly instead");
                    }
                }
            }
        }
        private void workDoneButton_Click(object sender, EventArgs e)
        {
            if (errored)
            {
                this.Close();
                return;
            }
            tabControl.SelectedIndex = 2;
        }

        void WorkDone(bool success = true)
        {
            if (!success)
            {
                errored = true;
                workDoneButton.Text = "Error";
            }
            else
            {
                workDoneButton.Text = "Continue";
            }
            workDoneButton.Enabled = true;
        }

        private async void ReplaceAceWithGion()
        {
            try
            {
                if (File.Exists(audioPck_location + ".backup"))
                {
                    Log("AUDIO_Default.pck.backup already exists!");
                    if (MessageBox.Show("A backup file was already found. Would you like to restore the backup? (This will restore ACE)", "Restore Backup?", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        File.Delete(audioPck_location);
                        File.Move(audioPck_location + ".backup", audioPck_location);
                        Log("Backup Restored Successfully");
                        MessageBox.Show("Backup Restored!", "Success!");
                    }
                    Close();
                    return;
                }

                Log("Beginning operations");
                if (safeModeCheckBox.Checked)
                {
                    Log("Safe Mode was enabled, Checking MD5");
                    byte[] valid_md5 = { 0x5d, 0x95, 0x81, 0x27, 0xa1, 0x24, 0xa8, 0x94, 0xbc, 0x50, 0x08, 0xb6, 0xff, 0x07, 0x2f, 0x17 };
                    using (MD5 md5 = MD5.Create())
                    using (FileStream fs = new FileStream(audioPck_location, FileMode.Open, FileAccess.ReadWrite))
                    using (BufferedStream bs = new BufferedStream(fs, 16 * 1024))
                    {
                        byte[] fileHash = await Task.Run(() =>
                        {
                            return md5.ComputeHash(bs);
                        });

                        if (!fileHash.SequenceEqual(valid_md5))
                        {
                            Log("The selected AUDIO_Default.pck file has not been confirmed to work with this program! Stopping for safety!");
                            WorkDone(false);
                            return;
                        }
                        Log("MD5 matched! This file is valid");
                    }
                }

                //at this point we are sure AUDIO_Default.pck is the same as the reference one. Modify it accordingly
                Log("Backing up AUDIO_Default.pck to AUDIO_Default.pck.backup");
                File.Move(audioPck_location, audioPck_location + ".backup");

                byte[] gionWem_Data = new byte[1730662];

                const long aceWemLength_offset = 19824;
                const long gionWemContents_offset = 641238361;
                const long aceWemContents_offset = 598730059;

                //Open audio_default
                using (FileStream sourceFileStream = new FileStream(audioPck_location + ".backup", FileMode.Open, FileAccess.Read))
                {
                    sourceFileStream.Seek(gionWemContents_offset, SeekOrigin.Begin);
                    int leftToRead = gionWem_Data.Length;
                    int offset = 0;
                    while (leftToRead > 0)
                    {
                        int trueRead = await sourceFileStream.ReadAsync(gionWem_Data, offset, leftToRead);
                        leftToRead -= trueRead;
                        offset += trueRead;
                    }
                    Log("Found and copied GION to memory");

                    sourceFileStream.Seek(0, SeekOrigin.Begin);

                    using (FileStream destinationFileStream = new FileStream(audioPck_location, FileMode.Create, FileAccess.ReadWrite))
                    using (BinaryWriter binWriter = new BinaryWriter(destinationFileStream))
                    {
                        Log("Copying up to ACE audio");
                        //Copy up to the offset of the ACE wem file
                        leftToRead = (int)aceWemLength_offset;

                        await BufferedStreamCopy(sourceFileStream, destinationFileStream, leftToRead);

                        Log("Changing the length of ACE audio wem");
                        binWriter.Write((int)1730662);
                        sourceFileStream.Seek(4, SeekOrigin.Current); //skip past 4 bytes

                        Log("Copying up to beginning of ACE wem");
                        leftToRead = (int)aceWemContents_offset - (int)sourceFileStream.Position;

                        await BufferedStreamCopy(sourceFileStream, destinationFileStream, leftToRead);
                        Log("Replacing ACE with GION");
                        await destinationFileStream.WriteAsync(gionWem_Data, 0, gionWem_Data.Length);
                        sourceFileStream.Seek(800061, SeekOrigin.Current);

                        Log("Copying the rest of the file");

                        leftToRead = (int)(sourceFileStream.Length - sourceFileStream.Position);

                        await BufferedStreamCopy(sourceFileStream, destinationFileStream, leftToRead);

                        await FixAllFileOffsets(destinationFileStream);

                        Log("Done!");
                    }

                    if (safeModeCheckBox.Checked)
                    {
                        Log("Safe Mode was enabled, Checking MD5 of result file ");
                        byte[] valid_md5 = { 0x3b, 0x46, 0x0e, 0xc8, 0xdd, 0xed, 0x06, 0x83, 0x38, 0x67, 0x3e, 0x2c, 0xd5, 0x70, 0x31, 0x97 };
                        bool success = false;
                        using (MD5 md5 = MD5.Create())
                        using (FileStream fs = new FileStream(audioPck_location, FileMode.Open, FileAccess.Read))
                        using (BufferedStream bs = new BufferedStream(fs, 16 * 1024))
                        {
                            success = await Task.Run(() =>
                            {
                                return md5.ComputeHash(bs).SequenceEqual(valid_md5);
                            });
                        }

                        if (!success)
                        {
                            Log("The resulting AUDIO_Default.pck file DID NOT match our expected output!");
                            if (MessageBox.Show("The resulting file did not match our expected output. This means something might have gone wrong.\nWould you like to restore the backup?", "Mismatch", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            {
                                Log("Restoring from Backup");
                                File.Delete(audioPck_location);
                                File.Copy(audioPck_location + ".backup", audioPck_location);
                            }
                            WorkDone();
                            return;
                        }
                        Log("MD5 matched! This file is valid");
                        WorkDone();
                    }
                }
                
            }
            catch (Exception e)
            {
                Log($"ERROR! Unexpected exception occured. {e}");
                return;
            }
        }

        Task FixAllFileOffsets(Stream str)
        {
            const uint wemFilesListings_offset = 18212;
            const uint OffsetDisparity = 930601;
            const uint nameOfAceWem = 190629806;
            const uint nameOfGionWem = 878886956; //not used, just here to make finding offsets easier :)

            return Task.Run(() =>
            {
                Log("Fixing the offsets for remaining clips...");
                using (BinaryWriter writer = new BinaryWriter(str, Encoding.ASCII, true))
                using (BinaryReader reader = new BinaryReader(str, Encoding.ASCII, true))
                {
                    str.Position = wemFilesListings_offset;
                    uint numberOfTotalFilesInPck = reader.ReadUInt32();
                    bool startCorrectingOffsets = false;
                    int correctedFiles = 0;
                    for (int i = 0; i < numberOfTotalFilesInPck; i++)
                    {
                        if (!startCorrectingOffsets)
                        {
                            //find ace wem
                            uint nameOfCurrentFile = reader.ReadUInt32();
                            Log($"Found file {nameOfCurrentFile}");
                            if (nameOfCurrentFile == nameOfAceWem)
                            {
                                Log($"Found index of ACE wem! It is located at {str.Position - 4}");
                                startCorrectingOffsets = true;
                            }
                            str.Position += 16;//skip the other 3 remaining uint entries
                        }
                        else
                        {
                            uint currentFileName = reader.ReadUInt32();
                            uint multiplier = reader.ReadUInt32();
                            uint length = reader.ReadUInt32();
                            uint currentPosition = reader.ReadUInt32();
                            currentPosition += OffsetDisparity;
                            str.Position -= 4;
                            writer.Write(currentPosition);

                            uint directoryId = reader.ReadUInt32();
                            correctedFiles++;
                        }
                    }
                    Log($"Offsets fixed! Corrected a total of {correctedFiles} files");
                }
            });
        }

        async Task BufferedStreamCopy(Stream source, Stream target, int toCopy)
        {
            byte[] buffer = new byte[16 * 1024];
            int leftToRead = toCopy;

            while (leftToRead > 0)
            {
                int trueRead = await source.ReadAsync(buffer, 0, leftToRead > buffer.Length ? buffer.Length : leftToRead);
                await target.WriteAsync(buffer, 0, trueRead);

                leftToRead -= trueRead;
            }
        }


        private void Log(string message)
        {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)(() => Log(message)));
            }
            else
            {
                logTextBox.AppendText(message + Environment.NewLine);
            }
        }
    }
}

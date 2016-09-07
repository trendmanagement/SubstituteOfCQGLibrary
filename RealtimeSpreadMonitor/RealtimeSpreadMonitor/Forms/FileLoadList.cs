using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RealtimeSpreadMonitor.Forms
{
    public partial class FileLoadList : Form
    {
        private OptionRealtimeMonitor optionRealtimeMonitor;

        public FileLoadList(OptionRealtimeMonitor optionRealtimeMonitor)
        {
            this.optionRealtimeMonitor = optionRealtimeMonitor;

            InitializeComponent();

            listViewFileList.SmallImageList = imageList1;
            listViewFileList.View = View.SmallIcon;
        }

        delegate void updateLoadingFTPfilesProgressBarDelegate(int progress);

        public void updateLoadingFTPfilesProgressBar(int progress)
        {
            if (this.InvokeRequired)
            {
                updateLoadingFTPfilesProgressBarDelegate d =
                    new updateLoadingFTPfilesProgressBarDelegate(
                        updateloadingFTPfilesThreadSafe);

                this.Invoke(d, progress);
            }
            else
            {
                updateloadingFTPfilesThreadSafe(progress);
            }
        }

        private void updateloadingFTPfilesThreadSafe(int progress)
        {
            loadingFTPfiles.Value = progress;
        }

        delegate void ThreadSafeloadFilesdelegate(String file);

        public void loadFiles(String file)
        {
            if (this.InvokeRequired)
            {
                ThreadSafeloadFilesdelegate d =
                    new ThreadSafeloadFilesdelegate(loadFilesThreadSafe);

                this.Invoke(d, file);
            }
            else
            {
                loadFilesThreadSafe(file);
            }
        }

        private void loadFilesThreadSafe(String file)
        {
            if (file.EndsWith(".xls")
                    || file.EndsWith(".xlt") || file.EndsWith(".csv"))
            {
                bool import = true;

                foreach (ListViewItem lvi in listViewFileList.Items)
                {
                    if(file == lvi.Text)
                    {
                        import = false;
                    }

                }

                if (import)
                {

                    System.IO.FileInfo fileInfo = new System.IO.FileInfo(file);

                    ListViewItem item = new ListViewItem(fileInfo.FullName, 1);

                    Icon iconForFile = SystemIcons.WinLogo;

                    if (fileInfo.FullName[0] != 92)
                    {
                        iconForFile = Icon.ExtractAssociatedIcon(fileInfo.FullName);


                        if (!imageList1.Images.ContainsKey(fileInfo.Extension))
                        {
                            // If not, add the image to the image list.
                            iconForFile = System.Drawing.Icon.ExtractAssociatedIcon(fileInfo.FullName);
                            imageList1.Images.Add(fileInfo.Extension, iconForFile);
                        }
                    }

                    item.ImageKey = fileInfo.Extension;
                    listViewFileList.Items.Add(item);

                    //addListViewItemsThreadsafe(item);
                }
            }
        }

        //delegate void ThreadSafeAddListViewItems(ListViewItem item);

        //public void addListViewItemsThreadsafe(ListViewItem item)
        //{
        //    if (this.InvokeRequired)
        //    {
        //        ThreadSafeAddListViewItems d =
        //            new ThreadSafeAddListViewItems(addListViewItems);

        //        this.Invoke(d, item);
        //    }
        //    else
        //    {
        //        addListViewItems(item);
        //    }
        //}

        //private void addListViewItems(ListViewItem item)
        //{
        //    listViewFileList.Items.Add(item);
        //}

        private void btnClearFiles_Click(object sender, EventArgs e)
        {
            listViewFileList.Items.Clear();
        }

        private void FileLoadList_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.All;
        }

        private void FileLoadList_DragDrop(object sender, DragEventArgs e)
        {
            optionRealtimeMonitor.draggingFileCheck(sender, e);
        }

        private void btnLoadFiles_Click(object sender, EventArgs e)
        {
            if (listViewFileList.Items != null)
            {
                String[] importFileList = new String[listViewFileList.Items.Count];

                int listCounter = 0;

                foreach (ListViewItem lvi in listViewFileList.Items)
                {
                    importFileList[listCounter] = lvi.Text;

                    listCounter++;
                }

                optionRealtimeMonitor.loadFiles(importFileList);

                optionRealtimeMonitor.closeFileLoadList();
            }

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            optionRealtimeMonitor.closeFileLoadList();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem lvi in listViewFileList.Items)
            {
                if (lvi.Selected)
                {
                    lvi.Remove();
                }
            }
        }

        private void listViewFileList_MouseClick(object sender, MouseEventArgs e)
        {
#if DEBUG
            try
#endif
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    loadFileListMenuStrip.Show(listViewFileList, new Point(e.X, e.Y));
                }
            }
#if DEBUG
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }
#endif
        }
    }
}

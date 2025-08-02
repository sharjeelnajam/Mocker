using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media.Imaging;
using MockerProject.ViewModels;
using System;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace MockerProject.Views
{
    public partial class ShareWindow : Window
    {
        static MainWindowViewModel m_MainVM;
        static ShareWindow m_ShareWindow;
        
        public ShareWindow()
        {
            InitializeComponent();
            m_ShareWindow = this;
        }
        
        public void setMainVM(MainWindowViewModel mainVM)
        {
            m_MainVM = mainVM;
            for (int i = 0; i < m_MainVM.SmallScreens.Count; i++)
            {
                m_MainVM.SmallScreenID = i;
                ScreenSmallView page = new ScreenSmallView();
                page.SmallCanvasText.Text = m_MainVM.SmallScreens[i].SmallCanvasText.Text;
                page.ScreenUnSaved.IsEnabled = false;
                page.ScreenUnSaved.Text = "";
                page.Margin = new Thickness(10);
                page.iamge.Source = m_MainVM.getImage(i);
                pagePanel.Children.Add(page);
            }
            ShareName.Text = m_MainVM.m_strProjectTitle;
        }

        private void onShare(object? sender, RoutedEventArgs e)
        {
            /*var renderTargetBitmap = new RenderTargetBitmap(new PixelSize((int)1000, (int)1500));
            Canvas canvas1 = m_MainVM.WorkScreen.screenCanvas;
            renderTargetBitmap.Render(m_MainVM.WorkScreen.rect);
            Image image = new Image();// This is a Image
            image.Source = renderTargetBitmap;
            image.Width = 180;
            image.Height = 150;
            image.Stretch = Stretch.Fill;
            using (var stream = new FileStream("temp\\Screenshot.png", FileMode.Create))
            {
                renderTargetBitmap.Save(stream);
            }
            
            var rander = new RenderTargetBitmap(new PixelSize((int)300, (int)400));
            rander.Render(m_MainVM.m_ScreenSmallView[0].SmallCanvas);
            // Save the PNG image to a file
            using (var stream = new FileStream("temp\\Screenshot1.png", FileMode.Create))
            {
                rander.Save(stream);
            }*/

            string path = makeProject(); ;
            string UploadPath = makeZip(path);
            string desc = "", tag = "", pw = "";
            if (ShareDescription.Text != null) desc = ShareDescription.Text;
            if (ShareTag.Text != null) tag = ShareTag.Text;
            if (SharePW.Text != null) pw = SharePW.Text;
            onUpload(UploadPath, shareTab, desc, tag, pw);
        }

        private string makeZip(string path)
        {
            // Create FileStream for output ZIP archive
            string w_strZipPath = path + ".zip";
            if (File.Exists(w_strZipPath))
                File.Delete(w_strZipPath);
            ZipFile.CreateFromDirectory(path, w_strZipPath);
            //ZipFile.CreateFromDirectory(path, w_strZipPath, CompressionLevel.Fastest, true);
            return w_strZipPath;
        }

        private void makeScreenShots(string path)
        {
            m_MainVM.createFolder(path);
            string filePath;
            for (int i = 0; i < m_MainVM.m_lstWorkScreen.Count; i++)
            {
                filePath = path + "\\" + m_MainVM.m_lstWorkScreen[i].m_strName + ".png";
                var rander = new RenderTargetBitmap(new PixelSize((int)180, (int)150));
                rander.Render(m_MainVM.m_ScreenSmallView[i].SmallCanvas);
                // Save the PNG image to a file
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    rander.Save(stream);
                }
            }
        }

        private void makeAssets(string path)
        {
            m_MainVM.createFolder(path);
            UIControl w_uiControl;
            foreach (ScreenView screenView in m_MainVM.m_lstWorkScreen)
            {
                for (int i = 3; i < screenView.screenCanvas.Children.Count; i++)
                {
                    w_uiControl = (UIControl)screenView.screenCanvas.Children[i];
                    if (w_uiControl.GetType() == typeof(ImageControl))
                    {
                        string w_path = Path.Combine(path, Path.GetFileName(w_uiControl.m_strSrc));
                        if (!File.Exists(w_uiControl.m_strSrc)) continue;
                        File.Copy(w_uiControl.m_strSrc, w_path, true);
                    }
                }
            }
        }

        private string makeProject()
        {
            string w_strDirPath = "temp\\";
            w_strDirPath += m_MainVM.strProjectTitle;
            if (Directory.Exists(w_strDirPath))
                Directory.Delete(w_strDirPath, true);
            m_MainVM.createFolder(w_strDirPath);
            makeScreenShots(Path.Combine(w_strDirPath, "screenshots"));
            makeAssets(Path.Combine(w_strDirPath, "assets"));
            m_MainVM.saveAllPages(w_strDirPath);
            return w_strDirPath;
        }

        static async Task onUpload(string path, TabControl share, string desciption, string tag, string pw)
        {
            string w_strFilePath = path;

            using (HttpClient client = new HttpClient())
            {
                using (var formData = new MultipartFormDataContent())
                {
                    formData.Add(new StringContent("Harry Jel"), "Name");
                    formData.Add(new StringContent("1"), "Id");
                    formData.Add(new StringContent("1"), "Type");
                    formData.Add(new StringContent(desciption), "Description");
                    formData.Add(new StringContent(tag), "Tag");
                    formData.Add(new StringContent(pw), "Password");

                    // Read the file as a stream
                    using (FileStream fileStream = File.OpenRead(w_strFilePath))
                    {
                        // Create a stream content from the file stream
                        var fileContent = new StreamContent(fileStream);
                        fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                        {
                            Name = "file",
                            FileName = Path.GetFileName(w_strFilePath)
                        };

                        // Add the file content to the form data
                        formData.Add(fileContent);

                        // Send the form data to the API endpoint
                        share.IsEnabled = false;
                        var response = new HttpResponseMessage();
                        try
                        {
                            response = await client.PostAsync("https://localhost:44364/Mocker/Upload", formData);
                            if (response.IsSuccessStatusCode)
                            {
                                var url = await response.Content.ReadAsStringAsync();
                                Console.WriteLine("File upload successful!");
                                m_ShareWindow.Close();
                                var window = new UploadResponse();
                                window.shareLink.Text = url;
                                window.ShowDialog(m_MainVM.m_MainWindow);
                            }
                            else
                            {
                                m_ShareWindow.Close();
                                Console.WriteLine("File upload failed: " + response.ReasonPhrase);
                            }
                        }
                        catch (Exception e)
                        {
                            share.IsEnabled = true;
                        }
                    }
                }
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;

namespace ASP_presentation
{
    public partial class _Default : Page
    {
        private const string FilePath = "/Files/Img/";
        private const string ValidImgFormats = "jpg|JPG|jpeg|JPEG|png|PNG|bmp|BMP";

        private const string Newline = "<br/>";
        private const string NoFileSelected = "Файл не выбран.";
        private const string NoValidImage = "Принимаются только файлы следующих форматов: jpg|JPG|jpeg|JPEG|png|PNG|bmp|BMP!";
        private const string UploadStatus = "Статус загрузки: ";
        private const string IsUploaded = " - загружен!";

        private const string ColorGreen = "#1f8c11";
        private const string ColorRed = "#990000";

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.MaintainScrollPositionOnPostBack = true;
            FillImageSelectionPanel();
        }

        protected void Image_Click(object sender, EventArgs e)
        {
            SetPreviewImg((sender as ImageButton).CommandArgument);
        }

        protected void UploadButton_Click(object sender, EventArgs e)
        {
            UploadFilesOnServer(FileUploadControl);
        }

        protected void ClearButton_Click(object sender, EventArgs e)
        {
            ClearLabel(StatusUpload);
            ClearLabel(LabelDownload);
        }

        void UploadFilesOnServer(FileUpload fileUpload)
        {
            if (fileUpload.HasFile)
            {
                foreach (HttpPostedFile uploadFile in fileUpload.PostedFiles)
                    LoadFileNCheckError(uploadFile);
                FillImageSelectionPanel();
            }
            else
                StatusUpload.Text = HexFontColorText(NoFileSelected, ColorRed);
        }

        private void LoadFileNCheckError(HttpPostedFile file)
        {
            if (IsValidImage(file.FileName))
            {
                file.SaveAs(Path.Combine(Server.MapPath("~" + FilePath), file.FileName));
                StatusUpload.Text += UploadStatus + file.FileName + HexFontColorText(IsUploaded, ColorGreen) + Newline;
            }
            else
                StatusUpload.Text += HexFontColorText("ERROR", ColorRed) + " " +
                                     HexFontColorText(file.FileName, "#330000") + " " +
                                     HexFontColorText(NoValidImage, ColorRed) + Newline; 
        }

        private string HexFontColorText(string message, string hexColor)
        {
            return string.Format("<font color=\"{0}\">{1}</font>", hexColor, message);
        }

        private void SetPreviewImg(string filename)
        {
            Image_Preview.ImageUrl = filename;
        }

        void ClearLabel(Label label)
        {
            label.Text = String.Empty;
        }

        public void FillImageSelectionPanel()
        {
            List<Img> imgList = new List<Img>();
            foreach (string filePath in Directory.GetFiles(Server.MapPath("~" + FilePath)))
            {
                imgList.Add(new Img() { Name = FilePath + Path.GetFileName(filePath) });
                RepeaterImages.DataSource = imgList;
                RepeaterImages.DataBind();
            }
        }

        public static bool IsValidImage(string fileName)
        {
            Regex regex = new Regex(string.Format(@"(.*?)\.({0})$", ValidImgFormats));
            return regex.IsMatch(fileName);
        }

        protected void DownloadButton_Click(object sender, EventArgs e)
        {
            SaveTextInImg(Image_Preview.ImageUrl, textsArr.Value, colors.Value, posX.Value, posY.Value);
        }

        private void SaveTextInImg(string filename, string textsArr, string colorsArr, string posXarr, string posYarr)
        {
            string[] texts;
            string[] colors;
            string[] posX;
            string[] posY;

            texts = textsArr.Split(',');
            colors = colorsArr.Split(',');
            posX = posXarr.Split(',');
            posY = posYarr.Split(',');

            if (filename != "")
            {
                string pathTofile = Server.MapPath("") + filename;
                using (var image = System.Drawing.Image.FromFile(pathTofile.Replace("/","\\")))
                {
                    using (var graphics = Graphics.FromImage(image))
                    {
                        var textBounds = graphics.VisibleClipBounds;

                        for (int i = 0;  i < texts.Length; i++)
                        {
                            SolidBrush brush;
                            try
                            {
                                if (colors[i] == "black") brush = new SolidBrush(Color.Black);
                                else brush = new SolidBrush(Color.White);
                                graphics.DrawString(
                                texts[i],
                                new Font("Verdana", 40),
                                brush,
                                float.Parse(posX[i], CultureInfo.InvariantCulture), float.Parse(posY[i], CultureInfo.InvariantCulture));
                            }                          
                            catch(Exception)
                            {
                            }
                        }  
                    }

                    image.Save(Server.MapPath("~/Files/Img/j.jpg"));
                }
            }
            else
                LabelDownload.Text = HexFontColorText(NoFileSelected, ColorRed);
        }
    }
}

public class Img
{
    public string Name { get; set; }
}
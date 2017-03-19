using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;

namespace FullCalendar_MVC.Utilities
{

    public class Common
    {
        /// <summary>
        /// Returns the comment text to be saved
        /// </summary>
        /// <returns>Returns the comments to be saved</returns>
        public static string ProcessPublicComments(HttpPostedFileBase file, int GalleryID)
        {
            string comments = string.Empty;

            string commentFilePath = string.Empty;
            if (ConfigurationManager.AppSettings["TempFilePath"] != null)
            {
                commentFilePath = ConfigurationManager.AppSettings["TempFilePath"];
            }
                         string fileType = Path.GetExtension(file.FileName);
                         string newFileName = GalleryID.ToString() + fileType;
            string originalFileName = newFileName;
            var originalFilePath = commentFilePath + originalFileName;
            if (File.Exists(originalFilePath)) File.Delete(originalFilePath);
            if (file != null)
            {
                if (file.ContentLength > 0)
                {
                    var path = commentFilePath + newFileName;
                    file.SaveAs(path);
                    comments = path;

                }
            }

            return comments;


        }

        /// <summary>
        /// Returns the Name of file to be saved
        /// </summary>
        /// <returns>Returns the Name of file to be saved</returns>
        public static string SavePSSummaryFiles(HttpPostedFileBase file, int ChemicalFileID)
        {
            string comments = string.Empty;

            string commentFilePath = string.Empty;
            if (ConfigurationManager.AppSettings["CommentFilePath"] != null)
            {
                commentFilePath = ConfigurationManager.AppSettings["CommentFilePath"];
            }
            commentFilePath = commentFilePath + "/PS/publicPSreport/";
            string originalFileName = file.FileName;
            var originalFilePath = commentFilePath + originalFileName;
            if (File.Exists(originalFilePath)) File.Delete(originalFilePath);

            //SqlConnection Conn= new SqlConnection(ConfigurationManager.ConnectionStrings["ACCContext"].ConnectionString);

            if (file.ContentLength > 0)
            {
                string fileType = Path.GetExtension(file.FileName);
                //DataTable dt =  Models.General.GeneralData.spData("spPS_Products_ChemicalFileUpload", Conn, new SqlParameter("@strExtnsn", fileType), new SqlParameter("@strFileNmRl", originalFileName));  

                string newFileName = ChemicalFileID.ToString() + fileType;
                var path = commentFilePath + newFileName;
                file.SaveAs(path);
                comments = newFileName;
            }

            return comments;


        }

        #region General Functions
        private const char cleanFieldDelimiter = '|';

        public static string cleanField(string strValue, string strRemove, bool blNeedsParenthesis, bool blNullIsZero = false)
        {
            string strCleanValue;

            if (strValue.Length <= 0)
            {
                if (blNullIsZero)
                    strCleanValue = "0";
                else
                    strCleanValue = "NULL";
            }
            else
            {
                var aryRemove = strRemove.Split(cleanFieldDelimiter);

                for (var i = 0; i <= aryRemove.Length - 1; i++)
                {
                    if (aryRemove[i].Length > 0) // verify string to be replaced isn't empty
                        strValue = strValue.Replace(aryRemove[i], "");
                }

                if (blNeedsParenthesis)
                    strCleanValue = "'" + strValue + "'";
                else
                    strCleanValue = strValue;
            }

            return strCleanValue;
        }

        public static string cleanField(string strValue, string strRemove, string strReplace, bool blNeedsParenthesis, bool blNullIsZero = false)
        {
            string strCleanValue;

            if (strValue.Length <= 0)
            {
                if (blNullIsZero)
                    strCleanValue = "0";
                else
                    strCleanValue = "NULL";
            }
            else
            {
                strValue = strValue.Replace(strRemove, strReplace);

                if (blNeedsParenthesis)
                    strCleanValue = "'" + strValue + "'";
                else
                    strCleanValue = strValue;
            }

            return strCleanValue;
        }

        #endregion
    }
}
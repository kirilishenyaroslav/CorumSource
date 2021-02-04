
using System.Collections.Generic;
using System.Web;


namespace Corum.Models.ViewModels
{
    public class ImportError
    {
        public string NumRow { get; set; }
        public string ColumnName { get; set; }
        public string CommentError { get; set; }
        public int? IsCommentType { get; set; }
    }



    public class ImportErrorsInfo
    {
        public int snapshotId { get; set; }
        public string logId { get; set; }
    }


    public class UploadFileViewModel
    {
        public bool IsRestFile { get; set; }
        public bool IsDocsFile { get; set; }
        public bool IsOrdersFile { get; set; }
        public bool IsTruckOrdersFile { get; set; }
        public int FileType { get; set; }
        public int SelectedColumnName { get; set; }
        public IList<ColumnSettingsModel>  CurrentSettings { get; set; }
        public IList<ColumnNameModel> ColumnNameList { get; set; }
        public IList<ColumnSettingsModel> CurrentSettingsRests { get; set; }
        public IList<ColumnSettingsModel> CurrentSettingsDocs { get; set; }

    }


    public class ColumnsFromExternalFile
    {
        public string ServerFileName { get; set; }
        public string RealName { get; set; }
        public bool IsRestFile { get; set; }
        public int FileType { get; set; }
        public List<string> Headers { get; set; }
        public List<string> InnerSPparams { get; set; }
        public Dictionary<string, string> InnerSPparamsDict { get; set; }

        public ColumnsFromExternalFile()
        {
            Headers = new List<string>();
            InnerSPparams = new List<string>();
            InnerSPparamsDict = new Dictionary<string, string>();
        }
    }

    public class ConfiguredByUserColumsPairsModel
    {
        public Dictionary<string, string> configuredPairs { get; set; }
        public string ServerFileName { get; set; }
        public int FileType { get; set; }
    }

    //класс для отбора полей таблиц для импорта
    public class ImportTemplateInfo
    {
        public string ColumnNameInDB { get; set; }
        public string ColumnType { get; set; }
        public string ColumnNameInFile { get; set; }
        public int IndexColumnInFile { get; set; }
        public string ColumnDescription { get; set; }
    }

}
   

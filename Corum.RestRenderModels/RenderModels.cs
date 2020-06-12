using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corum.RestRenderModels
{
    public struct HeaderItemInfo
    {

        public String columnName;
        public int columnOrder;
        public String columnField;
        public int columnWidth;
        public bool ColumnBlockStart;
        public bool ColumnBlockEnd;
        public int ColumnType;  //0 - decimal, 1 - int, 2 - data, 3 - string, 4 - time
    }
    public class RestHeaderInfo
    {
        public List<HeaderItemInfo> Headers;

        public RestHeaderInfo()
        {
            this.Headers = new List<HeaderItemInfo>();
        }
    }
    public class RestDataInfo<T>
    {
        public List<T> Rows { set; get; }

        public RestDataInfo()
        {
            this.Rows = new List<T>();
        }
    }

    public class RestFooterInfo
    {
        public Dictionary<int, object> Footers { set; get; }

        public RestFooterInfo()
        {
            this.Footers = new Dictionary<int, object>();
        }

    }
    public class RestParamsInfo
    {
        public String MainHeader { get; set; }
        public String Language { get; set; }
        public String Address { get; set; }
        public Dictionary<String, String> Params { set; get; }
        public RestParamsInfo()
        {
            this.Params = new Dictionary<String, String>();
        }                 
    }
}

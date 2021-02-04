using System;


namespace Corum.DAL.Helpers
{
    public static class ConvertTypeHelpers 
    {
        //проверка на удачную конвертацию вне зависимости от типа столбца
        public static bool ConvertColumnVal(bool IsRest, int NumerRowInFile, string ColumnName, string ColumnType, ref object ColumnVal, ref string CommentError, ref string CommentChange, bool isNullCheck, bool isZeroNumericReplace, bool isZeroDateReplace, bool isNullForRestCheck)
        {
            bool SuccessConvert = true; //если тип string, то конвертация не нужна, всё норм.             
            if (isNullCheck)
                SuccessConvert = ColumnValIsNotNull(NumerRowInFile, ColumnName, ref ColumnVal, ref CommentError);

            if ((isNullForRestCheck) && (SuccessConvert))
                SuccessConvert = ColumnValIsNull(NumerRowInFile, ColumnName, ref ColumnVal, ref CommentError);

            if (SuccessConvert)
            {
                switch (ColumnType)
                {
                    case "decimal":
                        SuccessConvert = ConvertColumnValInDecimalType(IsRest, NumerRowInFile, ColumnName, ref ColumnVal,
                            ref CommentError, ref CommentChange, isZeroNumericReplace);
                        break;
                    case "int":
                        SuccessConvert = ConvertColumnValInIntType(IsRest, NumerRowInFile, ColumnName, ref ColumnVal,
                            ref CommentError, ref CommentChange, isZeroNumericReplace);
                        break;
                    case "datetime":
                        SuccessConvert = ConvertColumnValInDateTimeType(IsRest, NumerRowInFile, ColumnName,
                            ref ColumnVal, ref CommentError, ref CommentChange, isZeroDateReplace);
                        break;
                }
            }
            return SuccessConvert;
        }

        //проверка на удачную конвертацию столбца в тип "Decimal"
        public static bool ConvertColumnValInDecimalType(bool IsRest, int NumerRowInFile, string ColumnName,
            ref object ColumnVal, ref string CommentError, ref string CommentChange, bool isZeroNumericReplace)
        {
            if (((string) ColumnVal == "") && (isZeroNumericReplace))
            {
               // CommentChange = "не числовое значение (" + (string)ColumnVal + ")  в поле " + ColumnName +
               //     " в строке " + NumerRowInFile.ToString() + " было заменено на '0'";

                ColumnVal = "0";                
            }        

            bool SuccessConvert = true;
            decimal ColumnValAfterConvert;
            string ValueForConvert = ((string)ColumnVal).Replace(" ", string.Empty);
            SuccessConvert = decimal.TryParse(ValueForConvert, out ColumnValAfterConvert);
            if (SuccessConvert == true)
            {
                ColumnVal = ColumnValAfterConvert;
            }
            else
            {
                CommentError = "некорректная конвертация строкового значения("+ ValueForConvert + ") в тип decimal";
            }
            return SuccessConvert;
        }

        //проверка на удачную конвертацию столбца в тип "Int"
        public static bool ConvertColumnValInIntType(bool IsRest, int NumerRowInFile, string ColumnName, ref object ColumnVal, ref string CommentError, ref string CommentChange, bool isZeroNumericReplace)
        {
            if (((string) ColumnVal == "") && (isZeroNumericReplace))
            {
               // CommentChange = "не числовое значение (" + (string)ColumnVal + ")  в поле " + ColumnName +
               //     " в строке " + NumerRowInFile.ToString() + " было заменено на '0'";
                ColumnVal = "0";                
            }       

           // if ((string)ColumnVal == "") ColumnVal = "0";
            bool SuccessConvert = true;
            int ColumnValAfterConvert;
            string ValueForConvert = ((string)ColumnVal).Replace(" ", string.Empty);
            SuccessConvert = int.TryParse(ValueForConvert, out ColumnValAfterConvert);
            if (SuccessConvert == true)
                ColumnVal = ColumnValAfterConvert;
            else
            CommentError = "некорректная конвертация строкового значения("+ ColumnVal.ToString()+ ") в тип Integer";
            return SuccessConvert;
        }

        public static bool ConvertColumnValInDateTimeType(bool IsRest, int NumerRowInFile, string ColumnName,
            ref object ColumnVal, ref string CommentError, ref string CommentChange, bool isZeroDateReplace)
        {           
            bool SuccessConvert = true;            
            string valueForConvert = ((string)ColumnVal).Replace(" ", string.Empty);
            if (valueForConvert != "") 
            {
                DateTime columnValAfterConvert;
                SuccessConvert = DateTime.TryParse(valueForConvert, out columnValAfterConvert);
                if (SuccessConvert == true)                
                    ColumnVal = columnValAfterConvert;                
                else                
                    CommentError = "некорректная конвертация строкового значения(" + ColumnVal.ToString() +
                                   ") в тип Datetime";                
            }
            else
            {
                if (isZeroDateReplace)
                {
                   // CommentChange = "значение типа не дата (" + (string) ColumnVal + ")  в поле " + ColumnName +
                   //                 " в строке " + NumerRowInFile.ToString() + " было заменено на пустое";
                    ColumnVal = DBNull.Value;
                    SuccessConvert = true;
                }
                else
                {
                    CommentError = "некорректная конвертация строкового значения(" + ColumnVal.ToString() +
                                   ") в тип Datetime";
                    SuccessConvert = false;
                }
            }
            return SuccessConvert;
        }

        //проверка что поле не пустое
        public static bool ColumnValIsNotNull(int NumerRowInFile, string ColumnName, ref object columnVal, ref string commentError)                     
        {
            bool successCheck = true;    
            if ((string) columnVal == "")
            {
                successCheck = false;                
                commentError = "поле " + ColumnName +
                   " в строке " + NumerRowInFile.ToString() + " не должно быть пустым!";
            }
            return successCheck;
        }

        //проверка что поле пустое
        public static bool ColumnValIsNull(int NumerRowInFile, string ColumnName, ref object columnVal, ref string commentError)
        {
            bool successCheck = true;
            if ((string)columnVal != "")
            {
                successCheck = false;
                commentError = "поле " + ColumnName +
                   " в строке " + NumerRowInFile.ToString() + " должно быть пустым!";
            }
            return successCheck;
        }     
        
    }
}

using Inverter.Models;
using System.Globalization;

namespace Inverter.Data
{
    public static class MappingResponseModel
    {
        public static ResponseModel Mapping(this string value, ResponseModel responseModel)
        {
            try
            {
                var lines = value.Split('\n');
                List<string> currentData = null;

                for (int i = 0; i < lines.Length; i++)
                {
                    if (lines[i].Contains("JOB CONCLUDED"))
                    {
                        break;
                    }
                    if (lines[i].Contains("TRANSIENT ANALYSIS"))
                    {
                        i += 7; //skipp zawsze pustych linijek
                        if (currentData == null)
                        {
                            currentData = lines[i].Trim().Split(' ').ToList(); //trim usuwa \r
                            currentData = TrimWhiteSpace(currentData);
                            i++;
                        }
                        i++;
                    }
                    if (lines[i].Contains("Evaluation PSpice"))
                    {
                        currentData = null;
                    }
                    if (currentData != null)
                    {
                        var currentLine = lines[i].Trim().Split(' ').ToList();
                        currentLine = TrimWhiteSpace(currentLine);
                        if (currentLine.Count > 0)
                        {
                            for (int j = 0; j < responseModel.DataGraphs.Count; j++)
                            {
                                for (int k = 0; k < currentData.Count; k++)
                                {
                                    if (responseModel.DataGraphs[j].DataName.ToUpper() == currentData[k].ToUpper())
                                    {
                                        responseModel.DataGraphs[j].Y.Add(Single.Parse(currentLine[k], NumberStyles.Float, new CultureInfo("en-US")));
                                        responseModel.DataGraphs[j].X.Add(Single.Parse(currentLine[0], NumberStyles.Float, new CultureInfo("en-US")));
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Wystąpił błąd przy mapowaniu danych {Environment.NewLine} {ex.Message}");
            }
            return responseModel;
        }


        private static List<string> TrimWhiteSpace(List<string> values)
        {

            for (int i = 0; i < values.Count; i++)
            {
                if (values[i] == "")
                {
                    values.Remove(values[i]);
                    i--;
                }
            }
            return values;
        }
    }
}

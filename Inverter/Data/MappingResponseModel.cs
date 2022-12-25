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
                CultureInfo cultureInfo = new CultureInfo("en-US");
                var lines = value.Split('\n');
                List<string> currentData = null;

                for (int i = 0; i < lines.Length; i++)
                {
                    if (lines[i].Contains("JOB CONCLUDED"))
                    {
                        break;
                    }
                    TA(responseModel, cultureInfo, lines, ref currentData, ref i);
                    i = FFT(responseModel, cultureInfo, lines, i);

                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Wystąpił błąd przy mapowaniu danych {Environment.NewLine} {ex.Message}");
            }
            return responseModel;
        }

        private static void TA(ResponseModel responseModel, CultureInfo cultureInfo, string[] lines, ref List<string> currentData, ref int i)
        {
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
                            if (responseModel.DataGraphs[j].DataName == currentData[k])
                            {
                                responseModel.DataGraphs[j].Y.Add(Single.Parse(currentLine[k], NumberStyles.Float, cultureInfo));
                                responseModel.DataGraphs[j].X.Add(Single.Parse(currentLine[0], NumberStyles.Float, cultureInfo));
                            }
                        }
                    }
                }
            }
        }

        private static int FFT(ResponseModel responseModel, CultureInfo cultureInfo, string[] lines, int i)
        {
            if (lines[i].Contains("FOURIER COMPONENTS") && responseModel.DataGraphs.Any(x => x.DataName.Contains("fft")))
            {
                var name = TrimWhiteSpace(lines[i].Trim().Split(' ').ToList());
                int index = responseModel.DataGraphs.FindIndex(x => x.DataName.Replace("fft", "") == name.LastOrDefault() && x.DataName.Contains("fft"));
                responseModel.DataGraphs[index].Y.Add(0);
                responseModel.DataGraphs[index].X.Add(0);
                i += 9;
                for (int j = 0; j < responseModel.NumberOfHarmonic; j++)
                {
                    var currentLine = lines[i].Trim().Split(' ').ToList();
                    currentLine = TrimWhiteSpace(currentLine);

                    responseModel.DataGraphs[index].Y.Add(Single.Parse(currentLine[2], NumberStyles.Float, cultureInfo));
                    responseModel.DataGraphs[index].X.Add(Single.Parse(currentLine[1], NumberStyles.Float, cultureInfo));


                    if (lines[i].Contains("TOTAL HARMONIC DISTORTION"))
                    {
                        break;
                    }
                    i++;
                }
                i += 13;

            }

            return i;
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

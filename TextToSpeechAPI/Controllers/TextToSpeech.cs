using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CognitiveServices.Speech;
using System.Net.Http.Headers;
using System.Text;
using TextToSpeechAPI.Models;

namespace TextToSpeechAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TextToSpeech : Controller
    {
        private static IWebHostEnvironment _webHostEnvironment;

        public TextToSpeech(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        //[HttpGet]
        ////[Route("text")]
        //public async Task<IActionResult> GetSpeech()
        //{
        //    string speechKey = "ec9395f012124eddafcf19f4541e2c5d";
        //    string speechRegion = "centralus";
        //    var speechConfig = SpeechConfig.FromSubscription(speechKey, speechRegion);

        //    // The language of the voice that speaks.
        //    speechConfig.SpeechSynthesisVoiceName = "en-US-JennyNeural";

        //    using (var speechSynthesizer = new SpeechSynthesizer(speechConfig))
        //    {
        //        var speechSynthesisResult = await speechSynthesizer.SpeakTextAsync("aabb");
        //        return Ok(speechSynthesisResult);
        //    }

        //}
        [HttpPost]
        //[Route("text")]
        public async Task<IActionResult> GetSpeech([FromBody]Details details)
        {
            string speechKey = "ec9395f012124eddafcf19f4541e2c5d";
            string speechRegion = "centralus";
            var speechConfig = SpeechConfig.FromSubscription(speechKey, speechRegion);

            // The language of the voice that speaks.
            speechConfig.SpeechSynthesisVoiceName = "en-US-JennyNeural";

            using (var speechSynthesizer = new SpeechSynthesizer(speechConfig))
            {
                var speechSynthesisResult = await speechSynthesizer.SpeakTextAsync(details.data);
                return Ok(speechSynthesisResult);
            }

        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadFiles([FromForm] uploadFile obj)
        {
            if (!Directory.Exists(_webHostEnvironment.WebRootPath + "\\Resources\\"))
            {
                Directory.CreateDirectory(_webHostEnvironment.WebRootPath + "\\Resources\\");
            }
            using (FileStream fileStream = System.IO.File.Create(_webHostEnvironment.WebRootPath + "\\Resources\\" + obj.files.FileName))

            {
                obj.files.CopyTo(fileStream);
                //fileStream.Flush();
                string pdfPath = @"C:/Users/VINODH/Downloads/TextToSpeechAPI/TextToSpeechAPI/TextToSpeechAPI/wwwroot/Resources/sample1.pdf";
                using (PdfReader reader = new PdfReader(pdfPath))
                    for (int pagenumber = 1; pagenumber <= reader.NumberOfPages; pagenumber++)
                    {
                        ITextExtractionStrategy strategy = new SimpleTextExtractionStrategy();
                        string text = PdfTextExtractor.GetTextFromPage(reader, pagenumber, strategy);
                        text = Encoding.UTF8.GetString(ASCIIEncoding.Convert(Encoding.Default, Encoding.UTF8, Encoding.Default.GetBytes(text)));
                        GetSpeech(text);
                        return Ok(text);

                    }
                return Ok("\\Resources\\" + obj.files.FileName);
            }

            //string pdfPath = @"C:/Users/VINODH/Downloads/TextToSpeechAPI/TextToSpeechAPI/TextToSpeechAPI/wwwroot/Resources/obj.files.FileName";
            //using (PdfReader reader = new PdfReader(pdfPath))
            //    for (int pagenumber = 1; pagenumber <= reader.NumberOfPages; pagenumber++)
            //    {
            //        ITextExtractionStrategy strategy = new SimpleTextExtractionStrategy();
            //        string text=PdfTextExtractor.GetTextFromPage(reader, pagenumber,strategy);
            //        text=Encoding.UTF8.GetString(ASCIIEncoding.Convert(Encoding.Default,Encoding.UTF8,Encoding.Default.GetBytes(text)));
            //        GetSpeech(text);
            //        return Ok(text);
                    
            //    }


            return Ok();
        }

        private async void GetSpeech(string text)
        {
            string speechKey = "ec9395f012124eddafcf19f4541e2c5d";
            string speechRegion = "centralus";
            var speechConfig = SpeechConfig.FromSubscription(speechKey, speechRegion);



            // The language of the voice that speaks.
            speechConfig.SpeechSynthesisVoiceName = "en-US-JennyNeural";



            using (var speechSynthesizer = new SpeechSynthesizer(speechConfig))
            {
                var speechSynthesisResult = await speechSynthesizer.SpeakTextAsync(text);
                //return Ok(speechSynthesisResult);
            }
            //return Ok();
        }

    }
    public record Details(string data); 
}

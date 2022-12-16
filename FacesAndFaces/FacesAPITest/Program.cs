using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FacesAPITest
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            //var imagePath = @"C:\Users\USER\source\repos\.NetMicroservices\FacesAndFaces\FacesAPITest\oscars-2017.jpg";
            var imagePath = @"oscars-2017.jpg";
            var urlAddress = "http://localhost:6000/api/Faces";
            ImageUtility imgUtil = new ImageUtility();

            var bytes = imgUtil.ConvertToBytes(imagePath);

            List<byte[]> faceList = null;

            Tuple<List<byte[]>> facesResponse = null;



            var byteContent = new ByteArrayContent(bytes);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

            using(var httpClient = new HttpClient())
            {
                using(var response = await httpClient.PostAsync(urlAddress, byteContent))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    facesResponse = JsonConvert.DeserializeObject<Tuple<List<byte[]>>>(apiResponse);
                }
            }

            if(facesResponse.Item1.Count > 0)
            {
                for(int i = 0; i < facesResponse.Item1.Count; i++)
                {
                    imgUtil.FromBytesToImage(facesResponse.Item1[i], "faces" + i);
                }
            }
        }
    }
}

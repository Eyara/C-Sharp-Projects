using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using Newtonsoft.Json;

namespace WeatherModule
{
    class Program
    {
        static void Main(string[] args)
        {
            string City = "Tula";
            string Code = "ru";
            Weather MyClass = new Weather(City, Code);
            Console.WriteLine(MyClass.weatherDescription);
            Console.WriteLine(MyClass.weatherTemp);
            Console.WriteLine(MyClass.weatherWindSpeed);
            Console.ReadKey();
        }
    }
    class Weather
    {
        private string city;
        public string City
        {
            get
            {
                return city;
            }
            set
            {
                city = value;
            }
        }
        private string code;
        public string Code
        {
            get
            {
                return code;
            }
            set
            {
                code = value;
            }
        }
        private string id;
        public string sURL;
        private string sLine;
        private WebRequest wrGETURL;
        private Stream objStream;
        private StreamReader objReader;
        private static dynamic stuff;
        public string weatherDescription;
        public string weatherTemp;
        public string weatherWindSpeed;
        public Weather(string City, string Code)
        {
            id = "63c11595b601e61275ec878025b3593b";
            sURL = String.Format("http://api.openweathermap.org/data/2.5/weather?q={0},{1}&appid={2}&lang={3}&units={4}",
                    City, Code, id, Code, "metric");
            wrGETURL = WebRequest.Create(sURL);
            objStream = wrGETURL.GetResponse().GetResponseStream();
            objReader = new StreamReader(objStream);
            sLine = "";
            stuff = ParseWeather(sLine);
            weatherDescription = stuff["weather"][0]["description"];
            weatherTemp = String.Format("{0} °C", stuff["main"]["temp"]);
            weatherWindSpeed = String.Format("{0} м/c", stuff["wind"]["speed"]);
        }
        private dynamic ParseWeather(string sLine)
        {
            while (sLine != null)
            {
                sLine = objReader.ReadLine();
                if (sLine != null)
                    break;
            }
            dynamic stuff = Newtonsoft.Json.JsonConvert.DeserializeObject(sLine);
            return stuff;
        }
    }
}

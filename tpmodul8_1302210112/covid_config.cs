using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace tpmodul8_1302210112
{
    internal class covid_config
    {
        private static string GetCovidFilePath => Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "covid_config.json");

        public enum Suhu
        {
            Celcius = 1,
            Fahrenheit
        }

        public Suhu SatuanSuhu { get; private set; }
        public int BatasHariDemam { get; private set; }
        public string PesanDitolak { get; private set; }
        public string PesanDiterima { get; private set; }

        public covid_config()
        {
            var file = File.OpenText(GetCovidFilePath);
            Dictionary<string, JsonElement>? json = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(file.ReadToEnd());

            // Set property config
            UbahSatuan(json["satuan_suhu"].GetString());
            BatasHariDemam = json["batas_hari_demam"].GetInt32();
            PesanDitolak = json["pesan_ditolak"].GetString();
            PesanDiterima = json["pesan_diterima"].GetString();

        }

        
        public void MasukGedung()
        {
            Console.WriteLine($"\nBerapa suhu badan anda saat ini ? Dalam nilai {SatuanSuhu}");
            string? inputSuhu = Console.ReadLine();
            float suhu = -1;
            if (inputSuhu != null)
            {
                suhu = float.Parse(inputSuhu);
            }

            Console.WriteLine("Berapa hari yang lalu (perkiraan) anda terakhir memiliki gejala demam ?");
            string? inputHari = Console.ReadLine();
            int hariDemam = -1;
            if (inputHari != null)
            {
                hariDemam = int.Parse(inputHari);
            }

            CekSuhu(suhu, hariDemam);
        }

        
        public void CekSuhu(float suhu, int hariDemam)
        {
            bool suhuPass;
            bool hariPass = hariDemam < BatasHariDemam;

            switch (SatuanSuhu)
            {
                case Suhu.Celcius:
                    suhuPass = suhu >= 36.5 && suhu <= 37.5;
                    break;
                case Suhu.Fahrenheit:
                    suhuPass = suhu >= 97.7 && suhu <= 99.5;
                    break;
                default:
                    suhuPass = false;
                    break;
            }

            if (suhuPass && hariPass)
            {
                Console.WriteLine(PesanDiterima);
            }
            else
            {
                Console.WriteLine(PesanDitolak);
            }
        }

        
        public Suhu UbahSatuan(string satuanSuhu)
        {
            Suhu satuan;
            bool success = Enum.TryParse(satuanSuhu, true, out satuan);
            if (!success)
            {
                satuan = Suhu.Celcius;
            }

            return UbahSatuan(satuan);
        }

   
        public Suhu UbahSatuan(Suhu satuan)
        {
            SatuanSuhu = satuan;
            return satuan;
        }
    }
}

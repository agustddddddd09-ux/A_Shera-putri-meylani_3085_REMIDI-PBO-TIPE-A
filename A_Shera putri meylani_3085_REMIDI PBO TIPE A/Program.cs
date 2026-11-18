
using System;

namespace MBG
{
    // NO 1 

    public class Sekolah
    {
        public string idSekolah;
        public string namaSekolah;
        public int jumlahPaketDiterima;

        public Sekolah(string id, string nama, int jumlah)
        {
            idSekolah = id;
            namaSekolah = nama;
            jumlahPaketDiterima = jumlah;
        }

        public void TampilInfo()
        {
            Console.WriteLine("ID Sekolah: " + idSekolah);
            Console.WriteLine("Nama Sekolah: " + namaSekolah);
            Console.WriteLine("Jumlah Paket Diterima: " + jumlahPaketDiterima);
        }
    }


    // NO 2 & 5

    public abstract class PaketMBG
    {
        private string idPaket;
        private string namaPaket;
        private int stokDapur;

        public string IdPaket
        {
            get { return idPaket; }
        }

        public string NamaPaket
        {
            get { return namaPaket; }
        }

        public int StokDapur
        {
            get { return stokDapur; }
            private set
            {
                if (value >= 0)
                    stokDapur = value;
            }
        }

        public PaketMBG(string id, string nama, int stok)
        {
            if (stok < 0)
                throw new ArgumentException("Stok tidak boleh negatif!");

            idPaket = id;
            namaPaket = nama;
            stokDapur = stok;
        }

        public bool KurangiStok(int jumlah)
        {
            if (jumlah <= 0)
            {
                Console.WriteLine("Jumlah pengurangan harus lebih dari 0!");
                return false;
            }

            if (jumlah > stokDapur)
            {
                Console.WriteLine("Stok dapur tidak mencukupi!");
                return false;
            }

            stokDapur -= jumlah;
            return true;
        }

        public abstract decimal HitungBiaya(int jumlah);
    }

    // NO 3 - INHERITANCE

    public class PaketReguler : PaketMBG
    {
        public PaketReguler(string id, string nama, int stok)
            : base(id, nama, stok)
        {
        }

        public override decimal HitungBiaya(int jumlah)
        {
            return jumlah * 15000;
        }
    }


    public class PaketKhusus : PaketMBG
    {
        public string JenisDiet { get; set; }

        public PaketKhusus(string id, string nama, int stok, string jenisDiet)
            : base(id, nama, stok)
        {
            JenisDiet = jenisDiet;
        }

        public override decimal HitungBiaya(int jumlah)
        {
            return jumlah * 20000;
        }
    }


    // NO 6 - INTERFACE
    public interface DapatDidistribusikan
    {
        void KirimPaket();
    }


    public class DistribusiMBG : DapatDidistribusikan
    {
        private Sekolah sekolah;
        private PaketMBG paket;
        private int jumlah;

        public DistribusiMBG(Sekolah sekolah, PaketMBG paket, int jumlah)
        {
            this.sekolah = sekolah;
            this.paket = paket;
            this.jumlah = jumlah;
        }

        public void KirimPaket()
        {
            if (jumlah <= 0)
            {
                Console.WriteLine("Distribusi gagal! Jumlah paket harus lebih dari 0.");
                return;
            }

            if (jumlah > paket.StokDapur)
            {
                Console.WriteLine("Distribusi gagal! Stok dapur tidak mencukupi.");
                return;
            }

            bool berhasil = paket.KurangiStok(jumlah);

            if (berhasil)
            {
                sekolah.jumlahPaketDiterima += jumlah;

                Console.WriteLine(
                    "Distribusi MBG berhasil! Sebanyak "
                    + jumlah
                    + " paket makanan telah dikirim ke sekolah "
                    + sekolah.namaSekolah
                );
            }
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            // NO 4 - POLYMORPHISM

            PaketMBG paket1 = new PaketReguler(
                "P001", "Paket Reguler", 100
            );

            PaketMBG paket2 = new PaketKhusus(
                "P002", "Paket Khusus", 100, "Rendah Gula"
            );

            Console.WriteLine("=== PERHITUNGAN BIAYA ===");

            Console.WriteLine("Biaya Paket Reguler: Rp"
                + paket1.HitungBiaya(10));

            Console.WriteLine("Biaya Paket Khusus: Rp"
                + paket2.HitungBiaya(10));


            // NO 6 - PENGUJIAN DISTRIBUSI

            Console.WriteLine("\n=== DISTRIBUSI MBG ===");

            Sekolah sekolah1 = new Sekolah(
                "S001", "SD Negeri 1 Jember", 0
            );

            DistribusiMBG distribusi = new DistribusiMBG(
                sekolah1, paket1, 30
            );

            distribusi.KirimPaket();

            Console.WriteLine("\n=== INFORMASI SEKOLAH ===");

            sekolah1.TampilInfo();

            Console.WriteLine("Sisa Stok: " + paket1.StokDapur);
        }
    }
}


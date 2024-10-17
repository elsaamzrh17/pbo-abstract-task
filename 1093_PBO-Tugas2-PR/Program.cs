using System;

class Program
{
    static void Main(string[] args)
    {
        Robot robot1 = new RobotBiasa("Sarina", 100, 10, 20);
        Robot robot2 = new RobotBiasa("Clou", 80, 5, 25);
        BosRobot bosRobot = new BosRobot("PledisBoss", 150, 15);

        robot1.CetakInformasi();
        robot2.CetakInformasi();
        bosRobot.CetakInformasi();

        //Contoh Pertarungan
        robot1.Serang(robot2);
        robot2.Serang(robot1);
        bosRobot.Serang(robot1);

        //menggunakan kemampuan
        Kemampuan perbaikan = new Perbaikan();
        perbaikan.Gunakan(robot1, null);

        Kemampuan seranganListrik = new SeranganListrik();
        seranganListrik.Gunakan(bosRobot, robot2);

        //Cek kondisi mati
        if (robot2.Energi <= 0)
        {
            Console.WriteLine($"{robot2.Nama} telah mati.");
        }

        if (bosRobot.Energi <= 0)
        {
            bosRobot.Mati();
        }
    }
}

abstract class Robot
{
    public string Nama { get; set; }
    public int Energi { get; set; }
    public int Armor { get; set; }
    public int Serangan { get; set; }

    public Robot(string nama, int energi, int armor, int serangan)
    {
        Nama = nama;
        Energi = energi;
        Armor = armor;
        Serangan = serangan;
    }

    public virtual void Serang(Robot target)
    {
        if (Energi > 0)
        {
            int damage = Math.Max(0, Serangan - target.Armor);
            target.Energi -= damage;
            Console.WriteLine($"{Nama} menyerang {target.Nama} dengan damage {damage}. Energi {target.Nama} sekarang {target.Energi}.");
        }
        else
        {
            Console.WriteLine($"{Nama} tidak memiliki energi untuk menyerang.");
        }
    }

    public abstract void GunakanKemampuan(Kemampuan kemampuan);

    public void CetakInformasi()
    {
        Console.WriteLine($"Nama: {Nama}, Energi: {Energi}, Armor: {Armor}, Serangan: {Serangan}");
    }
}

class RobotBiasa : Robot
{
    private int seranganListrikCooldown = 0;
    public RobotBiasa(string nama, int energi, int armor, int serangan) : base(nama, energi, armor, serangan)
    {
    }

    public override void GunakanKemampuan(Kemampuan kemampuan)
    {
        if (kemampuan is SeranganListrik)
        {
            if (seranganListrikCooldown > 0)
            {
                Console.WriteLine($"{Nama} masih dalam cooldown untuk Serangan Listrik ({seranganListrikCooldown} turn tersisa).");
                return;
            }

            if (Energi >= 10)
            {
                Energi -= 10;
                int damage = 20;
                seranganListrikCooldown = 3; // set cooldown menjadi 3 turn
                Console.WriteLine($"{Nama} menggunakan Serangan Listrik dengan damage {damage}!");
            }
            else
            {
                Console.WriteLine($"{Nama} tidak memiliki cukup energi untuk menggunakan Serangan Listrik.");
            }
        }
        else
        {
            kemampuan.Gunakan(this, null);
        }

        // Mengurangi cooldown jika ada
        if (seranganListrikCooldown > 0)
        {
            seranganListrikCooldown--;
        }
    }
}


class BosRobot : Robot
{
    public int Pertahanan { get; set; }

    public BosRobot(string nama, int energi, int pertahanan) : base(nama, energi, 2 * pertahanan, 2 * (energi / 10))
    {
        Pertahanan = pertahanan;
    }

    public override void Serang(Robot target)
    {
        base.Serang(target);
    }

    public override void GunakanKemampuan(Kemampuan kemampuan)
    {
        if (kemampuan is SeranganListrik)
        {
            if (Energi >= 10)
            {
                Energi -= 10;
                int damage = 20;

                Console.WriteLine($"{Nama} menggunakan Serangan Api dengan damage {damage}!");
            }

            else
            {
                Console.WriteLine($"{Nama} tidak memiliki cukup energi untuk menggunakan Serangan Listrik.");
            }
        }
        else
        {
            kemampuan.Gunakan(this, null);
        }
    }

    public void Mati()
    {
        Console.WriteLine($"{Nama} telah mati.");
    }
}

interface Kemampuan
{
    void Gunakan(Robot pengguna, Robot target);
}

class Perbaikan : Kemampuan
{
    public void Gunakan(Robot pengguna, Robot target)
    {
        pengguna.Energi += 30;
        Console.WriteLine($"{pengguna.Nama} memulihkan energi. Total energi sekarang adalah {pengguna.Energi}");
    }
}

class SeranganListrik : Kemampuan
{
    public void Gunakan(Robot pengguna, Robot target)
    {
        if (pengguna.Energi >= 10)
        {
            pengguna.Energi -= 10;
            target.Energi -= 15;
            Console.WriteLine($"{pengguna.Nama} menggunakan Serangan Listrik pada {target.Nama}. Energi {target.Nama} sekarang {target.Energi}.");
        }

        else
        {
            Console.WriteLine($"{pengguna.Nama} tidak memiliki cukup energi untuk menggunakan Serangan Listrik.");
        }
    }
}

class SeranganPlasma : Kemampuan
{
    public void Gunakan(Robot pengguna, Robot target)
    {
        if (pengguna.Energi >= 10)
        {
            pengguna.Energi -= 10;
            target.Energi -= 15;
            Console.WriteLine($"{pengguna.Nama} menggunakan Serangan Plasma pada {target.Nama}. Energi {target.Nama} sekarang {target.Energi}.");
        }

        else
        {
            Console.WriteLine($"{pengguna.Nama} tidak memiliki cukup energi untuk menggunakan Serangan Plasma.");
        }
    }
}


class PertahananSuper : Kemampuan
{
    public void Gunakan(Robot pengguna, Robot target)
    {
        if (pengguna.Energi >= 5)
        {
            pengguna.Energi -= 5;
            pengguna.Armor += 10; //meningkatkan armor sementara
            Console.WriteLine($"{pengguna.Nama} menggunakan Pertahanan Super. Armor sekarang {pengguna.Armor}");
        }

        else
        {
            Console.WriteLine($"{pengguna.Nama} tidak memiliki cukup energi untuk menggunakan Pertahanan Super");
        }
    }
}
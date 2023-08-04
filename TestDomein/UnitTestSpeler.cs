using League.Domein.Domein;
using League.Domein.Exceptions;

namespace TestDomein
{
    public class UnitTestSpeler
    {
        [Fact]
        public void ZetId_valid()
        {
            Speler s = new Speler(10, "Jos", 180, 80);
            Assert.Equal(10, s.Id);
            s.ZetId(1);
            Assert.Equal(1, s.Id);

        }
        [Fact]
        public void ZetId_invalid()
        {
            Speler s = new Speler(10, "Jos", 180, 80);
            Assert.Equal(10, s.Id);
            Assert.Throws<SpelerException>(() => s.ZetId(0));
            Assert.Equal(10, s.Id);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(99)]
        public void ZetRugnummer_valid(int rugnummer)
        {
            Speler s = new Speler(10, "Jos", 180, 80);
            s.ZetRugnummer(rugnummer);
            Assert.Equal(rugnummer, s.Rugnummer);
        }
        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(100)]
        public void ZetRugnummer_invalid(int rugnummer)
        {
            Speler s = new Speler(10, "Jos", 180, 80);
            Assert.Throws<SpelerException>(() => s.ZetRugnummer(rugnummer));
        }
        [Theory]
        [InlineData("Jef","Jef")]
        [InlineData("Eden Hazard", "Eden Hazard")]
        [InlineData("Eden Hazard ", "Eden Hazard")]
        [InlineData("  Eden Hazard ", "Eden Hazard")]
        public void ZetNaam_valid(string naamin, string naamuit)
        {
            Speler s = new Speler(10, "Jos", 180, 80);
            s.ZetNaam(naamin);
            Assert.Equal(naamuit, s.Naam);
        }
        [Theory]
        [InlineData(" ")]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("  \n")]
        [InlineData("  \r")]
        public void ZetNaam_invalid(string naam)
        {
            Speler s = new Speler(10, "Jos", 180, 80);
            Assert.Throws<SpelerException>(() => s.ZetNaam(naam));
        }
        [Theory]
        [InlineData(10,"Jos",150,80,10,"Jos",150,80)]
        [InlineData(1, "Jos", 150, 50, 1, "Jos", 150, 50)]
        [InlineData(1, "Jos", null, 80, 1, "Jos", null, 80)]
        [InlineData(1, "Jos", 170, null, 1, "Jos", 170, null)]
        public void ctor_withID_valid(int id, string naam, int? lengte, int? gewicht, int vid, string vnaam, int? vlengte, int? vgewicht)
        {
            Speler s = new Speler(id, naam, lengte, gewicht);
            Assert.Equal(vid, s.Id);
            Assert.Equal(vnaam, s.Naam);
            Assert.Equal(vlengte, s.Lengte);
            Assert.Equal(vgewicht, s.Gewicht);
        }
        [Theory]
        [InlineData(0, "Jos", 150, 80)]
        [InlineData(1, " ", 150, 50)]
        [InlineData(1, "", 149, 50)]
        [InlineData(1, null, 150, 50)]
        [InlineData(1, " \n", 150, 50)]
        [InlineData(1, " \r", 150, 50)]
        [InlineData(1, "Jos", 149, 80)]
        [InlineData(1, "Jos", 170, 49)]
        public void ctor_withID_invalid(int id, string naam, int? lengte, int? gewicht)
        {
            Assert.Throws<SpelerException>(() => new Speler(id, naam, lengte, gewicht));
        }

        [Fact]
        public void verwijderTeam_valid()
        {
            Speler s = new Speler(10, "Jos", 180, 80);
            Team t = new Team(1,"Antwerpen");
            s.ZetTeam(t);
            s.VerwijderTeam();
            Assert.Null(s.Team);
            Assert.DoesNotContain(s, t.Spelers());
            
        }

        [Fact]
        public void ZetTeam_valid()
        {
            Speler s = new Speler(10, "Jos", 180, 80);
            Team t = new Team(1, "Antwerpen");
            Team t2 = new Team(1, "Gent");
            s.ZetTeam(t);
            Assert.Equal(t2, s.Team);
            Assert.Contains(s, t.Spelers());
        }

        [Fact]
        public void ZetTeam_invalid()
        {
            Speler s = new Speler(10, "Jos", 180, 80);
            Team t = new Team(1, "Antwerpen");
            s.ZetTeam(t);
            Assert.Throws<SpelerException>(() => s.ZetTeam(null));
            Assert.Throws<SpelerException>(() => s.ZetTeam(t));
            Assert.Equal(t, s.Team);
            Assert.Contains(s, t.Spelers());
        }


        


    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using projekat2.DomainModel;
using Neo4jClient;
using Neo4jClient.Cypher;
using Neo4jClient.ApiModels.Cypher;

namespace projekat2
{
    public partial class Form1 : Form
    {
        private GraphClient client;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            client = new GraphClient(new Uri("http://localhost:7474/db/data"), "neo4j", "ivana");
            try
            {
                client.Connect();
            }
                /*Dictionary<string, object> queryDict = new Dictionary<string, object>();
                var query = new Neo4jClient.Cypher.CypherQuery("MATCH (s:SKola) return o ORDER BY s.Skola", queryDict, CypherResultMode.Set);
                List<Skola> skole = ((IRawGraphClient)client).ExecuteGetCypherResults<Skola>(query).ToList();

                foreach (Skola s in skole)
                {
                    comboBox1.Items.Add(s.naziv);
               
                }

                var query1 = new Neo4jClient.Cypher.CypherQuery("MATCH (p:Predavac) return p ORDER BY p.ime", queryDict, CypherResultMode.Set);
                List<Predavac> predavaci = ((IRawGraphClient)client).ExecuteGetCypherResults<Predavac>(query1).ToList();

                foreach (Predavac p in predavaci)
                {
                    //comboBox2.Items.Add(p.ime);
           
                }

            }*/
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnPredavac_Click(object sender, EventArgs e)
        {
            DodajPredavaca dodajPredavacaForm = new DodajPredavaca();
            dodajPredavacaForm.client = client;
            dodajPredavacaForm.ShowDialog();
        }

        private void btnObrisiPredavaca_Click(object sender, EventArgs e)
        {
            if (txtObrisiPredavaca.Text != "")
            {
                Predavac predavac = new Predavac();
                predavac.ime = txtObrisiPredavaca.Text;

                Dictionary<string, object> queryDict = new Dictionary<string, object>();
                queryDict.Add("ime", predavac.ime);

                var query = new Neo4jClient.Cypher.CypherQuery("match (p:Predavac{ime:{ime}}) detach delete p", queryDict, CypherResultMode.Set);

                ((IRawGraphClient)client).ExecuteCypher(query);
            }
            txtObrisiPredavaca.Text = "";
            MessageBox.Show("Uspsesno ste izbrisali predavaca.");
        }

        private void btnObrisiKorisnika_Click(object sender, EventArgs e)
        {
            if (txtObrisiKorisnika.Text != "")
            {
                Korisnik korisnik = new Korisnik();
                korisnik.ime = txtObrisiKorisnika.Text;

                Dictionary<string, object> queryDict = new Dictionary<string, object>();
                queryDict.Add("ime", korisnik.ime);

                var query = new Neo4jClient.Cypher.CypherQuery("match (k:Korisnik{ime:{ime}}) detach delete k", queryDict, CypherResultMode.Set);

                ((IRawGraphClient)client).ExecuteCypher(query);
            }
            txtObrisiPredavaca.Text = "";
            MessageBox.Show("Uspsesno ste izbrisali korisnika.");
        }

        private void btnDodajKorisnika_Click(object sender, EventArgs e)
        {
            DodajKorisnika dodajKorisnikaForm = new DodajKorisnika();
            dodajKorisnikaForm.client = client;
            dodajKorisnikaForm.ShowDialog();
        }

        private void btnPronadjiSkolu_Click(object sender, EventArgs e)
        {
            string skola = ".*" + txtPronadjiSkolu.Text + ".*";

            Dictionary<string, object> queryDict = new Dictionary<string, object>();
            queryDict.Add("naziv", skola);

            var query = new Neo4jClient.Cypher.CypherQuery("match (s:Skola)  where s.naziv =~ {naziv} return s order by s.naziv",
                                                            queryDict, CypherResultMode.Set);

            try
            {

                List<Skola> skole = ((IRawGraphClient)client).ExecuteGetCypherResults<Skola>(query).ToList();

                Prikaz1 frm = new Prikaz1();
                frm.Value = skole;
                frm.ShowDialog();

                txtPronadjiSkolu.Text = "";

            }

            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        private void btnPrikaziPredavaca_Click(object sender, EventArgs e)
        {
            string predavac = ".*" + txtTraziPredavaca.Text + ".*";

            Dictionary<string, object> queryDict = new Dictionary<string, object>();
            queryDict.Add("ime", predavac);

            var query = new Neo4jClient.Cypher.CypherQuery("match (p:Predavac)  where p.ime =~ {ime} return p order by p.ime",
                                                            queryDict, CypherResultMode.Set);

            try
            {

                List<Predavac> predavaci = ((IRawGraphClient)client).ExecuteGetCypherResults<Predavac>(query).ToList();

                Prikaz2 frm = new Prikaz2();
                frm.Value = predavaci;
                frm.ShowDialog();

                txtTraziPredavaca.Text = "";

            }

            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        private void btnKurseviUSkoli_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text != "")
            {
                string skola = comboBox1.Text;

                Dictionary<string, object> queryDict = new Dictionary<string, object>();
                queryDict.Add("skola", skola);
                var query = new Neo4jClient.Cypher.CypherQuery("match (k:Kurs)-[r:ODRZAVA_SE]-(s:Skola) where s.naziv = {skola} return k",
                                                               queryDict, CypherResultMode.Set);

                List<Kurs> kursevi = ((IRawGraphClient)client).ExecuteGetCypherResults<Kurs>(query).ToList();

                PrikazKursa frm = new PrikazKursa();
                frm.Value = kursevi;
                frm.ShowDialog();
                comboBox1.Text = "";

            }
        }

        private void btnPredavacDrzi_Click(object sender, EventArgs e)
        {
            if (comboBox2.Text != "")
            {
                string predavac = comboBox2.Text;

                Dictionary<string, object> queryDict = new Dictionary<string, object>();
                queryDict.Add("predavac", predavac);
                var query = new Neo4jClient.Cypher.CypherQuery("match (p:Predavac)-[r:PREDAJE]-(k:Kurs) where p.ime = {predavac} return k",
                                                               queryDict, CypherResultMode.Set);

                List<Kurs> kursevi = ((IRawGraphClient)client).ExecuteGetCypherResults<Kurs>(query).ToList();

                PrikazKursa frm = new PrikazKursa();
                frm.Value = kursevi;
                frm.ShowDialog();
                comboBox2.Text = "";

            }
        }

        private void btnPrikaziKorisnika_Click(object sender, EventArgs e)
        {
            string korisnik = ".*" + txtImeKorisnika.Text + ".*";

            Dictionary<string, object> queryDict = new Dictionary<string, object>();
            queryDict.Add("korisnik", korisnik);

            var query = new Neo4jClient.Cypher.CypherQuery("match (k:Korisnik)  where k.ime =~ {korisnik} return k order by k.ime",
                                                            queryDict, CypherResultMode.Set);

            try
            {

                List<Korisnik> korisnici = ((IRawGraphClient)client).ExecuteGetCypherResults<Korisnik>(query).ToList();

                Prikaz3 frm = new Prikaz3();
                frm.Value = korisnici;
                frm.ShowDialog();

                txtImeKorisnika.Text = "";

            }

            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }

        }

        private void btnKorisnikPohadja_Click(object sender, EventArgs e)
        {
            if (comboBox4.Text != "")
            {
                string korisnik = comboBox4.Text;

                Dictionary<string, object> queryDict = new Dictionary<string, object>();
                queryDict.Add("korisnik", korisnik);
                var query = new Neo4jClient.Cypher.CypherQuery("match (k:Korisnik)-[r:POHADJA]-(n:Kurs) where k.ime = {korisnik} return n",
                                                               queryDict, CypherResultMode.Set);

                List<Kurs> kursevi = ((IRawGraphClient)client).ExecuteGetCypherResults<Kurs>(query).ToList();

                PrikazKursa frm = new PrikazKursa();
                frm.Value = kursevi;
                frm.ShowDialog();
                comboBox4.Text = "";

            }
        }

        private void btnPrikazi_Click(object sender, EventArgs e)
        {
            string grupa = ".*" + txtGiliI.Text + ".*";
            string jezik = ".*" + comboBox3.Text + ".*";

            Dictionary<string, object> queryDict = new Dictionary<string, object>();
            queryDict.Add("grupa", grupa);
            queryDict.Add("jezik", jezik);

            var query = new Neo4jClient.Cypher.CypherQuery("match (k:Kurs) where k.jezik =~ {jezik} and k.grupa=~ {grupa} return k",
                                                          queryDict, CypherResultMode.Set);

            try
            {

                List<Kurs> kursevi = ((IRawGraphClient)client).ExecuteGetCypherResults<Kurs>(query).ToList();

                PrikazKursa frm = new PrikazKursa();
                frm.Value = kursevi;
                frm.ShowDialog();

                txtGiliI.Text = "";
                comboBox3.Text = "";
            }

            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        private void btnTip_Click(object sender, EventArgs e)
        {
            string tip = ".*" + txtTip.Text + ".*";
            string kurs = ".*" + comboBox3.Text + ".*";

            Dictionary<string, object> queryDict = new Dictionary<string, object>();
            queryDict.Add("tip", tip);
            queryDict.Add("kurs", kurs);

            var query = new Neo4jClient.Cypher.CypherQuery("match (k:Kurs) where k.jezik =~ {kurs} and k.tip=~ {tip} return k",
                                                          queryDict, CypherResultMode.Set);

            try
            {

                List<Kurs> kursevi = ((IRawGraphClient)client).ExecuteGetCypherResults<Kurs>(query).ToList();

                PrikazKursa frm = new PrikazKursa();
                frm.Value = kursevi;
                frm.ShowDialog();

                txtGiliI.Text = "";
                comboBox3.Text = "";
            }

            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        private void btnPredavacUSkoli_Click(object sender, EventArgs e)
        {
            string skola = comboBox1.Text;
            Dictionary<string, object> queryDict = new Dictionary<string, object>();
            queryDict.Add("skola", skola);


            var query = new Neo4jClient.Cypher.CypherQuery("match(s:Skola)<-[r: ODRZAVA_SE]-(k:Kurs)-[r1:PREDAJE]->(p:Predavac) where s.naziv = { skola } return p ORDER BY p.ime",
                                                                   queryDict, CypherResultMode.Set);

            try
            {
                List<Predavac> predavaci = ((IRawGraphClient)client).ExecuteGetCypherResults<Predavac>(query).ToList();

                Prikaz2 frm = new Prikaz2();
                frm.Value = predavaci;
                frm.ShowDialog();

                comboBox1.Text = "";
            }

            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        private void btnKorisnikPredavac_Click(object sender, EventArgs e)
        {
            string predavac = comboBox2.Text;
            Dictionary<string, object> queryDict = new Dictionary<string, object>();
            queryDict.Add("predavac", predavac);


            var query = new Neo4jClient.Cypher.CypherQuery("match(p:Predavac)<-[r: PREDAJE]-(k:Kurs)-[r1: POHADJA]->(n:Korisnik) where p.ime = { predavac } return n ORDER BY n.ime",
                                                                   queryDict, CypherResultMode.Set);

            try
            {
                List<Korisnik> korisnici = ((IRawGraphClient)client).ExecuteGetCypherResults<Korisnik>(query).ToList();

                Prikaz3 frm = new Prikaz3();
                frm.Value = korisnici;
                frm.ShowDialog();

                comboBox2.Text = "";
            }

            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        private void btnKorisnikSkola_Click(object sender, EventArgs e)
        {
            string korisnik = comboBox4.Text;
            Dictionary<string, object> queryDict = new Dictionary<string, object>();
            queryDict.Add("korisnik", korisnik);


            var query = new Neo4jClient.Cypher.CypherQuery("match(k:Korisnik)<-[r: POHADJA]-(n:Kurs)-[r1: ODRZAVA_SE]->(s:Skola) where k.ime = { korisnik } return s ORDER BY s.naziv",
                                                                   queryDict, CypherResultMode.Set);

            try
            {
                List<Skola> skole = ((IRawGraphClient)client).ExecuteGetCypherResults<Skola>(query).ToList();

                Prikaz1 frm = new Prikaz1();
                frm.Value = skole;
                frm.ShowDialog();

                comboBox4.Text = "";
            }

            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }
    }
}

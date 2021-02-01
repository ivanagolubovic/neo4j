using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Neo4jClient;
using Neo4jClient.Cypher;
using projekat2.DomainModel;

namespace projekat2
{
    public partial class DodajKorisnika : Form
    {
        public GraphClient client;
        public DodajKorisnika()
        {
            InitializeComponent();
        }

        private void DodajKorisnika_Load(object sender, EventArgs e)
        {
            client = new GraphClient(new Uri("http://localhost:7474/db/data"), "neo4j", "ivana");
            try
            {
                client.Connect();
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
            //
            Dictionary<string, object> queryDict = new Dictionary<string, object>();

            try
            {

                var query = new Neo4jClient.Cypher.CypherQuery("MATCH (k:Kurs) return k.id ORDER BY k.id", queryDict, CypherResultMode.Set);
                List<string> kursevi = ((IRawGraphClient)client).ExecuteGetCypherResults<string>(query).ToList();

                comboBox1.DataSource =kursevi;
                comboBox1.Text = "";
            }

            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        private void btnDodajKorisnika_Click(object sender, EventArgs e)
        {
            Korisnik korisnik = this.createKorisnik();


            Dictionary<string, object> queryDict = new Dictionary<string, object>();
            queryDict.Add("ime", korisnik.ime);
            queryDict.Add("prezime", korisnik.prezime);
            queryDict.Add("telefon", korisnik.telefon);


            var query = new Neo4jClient.Cypher.CypherQuery("CREATE (n:Korisnik {ime : {ime}, prezime : {prezime}, telefon : {telefon}",
                                                           queryDict, CypherResultMode.Set);

            List<Korisnik> korisnici = ((IRawGraphClient)client).ExecuteGetCypherResults<Korisnik>(query).ToList();
            MessageBox.Show("Uspesno ste dodali korisnika.");
            foreach (Korisnik n in korisnici)
            {
                MessageBox.Show(n.ime);
            }


            this.Close();

        }

        private Korisnik createKorisnik()
        {
            Korisnik n = new Korisnik();

            n.ime = txtIme.Text;
            n.prezime = txtPrezime.Text;
            n.telefon = txtTelefon.Text;


            return n;
        }

        private void btnIzmeniKorisnika_Click(object sender, EventArgs e)
        {
            Korisnik korisnik = new Korisnik();

            korisnik.ime = txtIme.Text;
            korisnik.prezime = txtPrezime.Text;
            korisnik.telefon = txtTelefon.Text;

            Dictionary<string, object> queryDict = new Dictionary<string, object>();
            queryDict.Add("ime", korisnik.ime);
            queryDict.Add("prezime", korisnik.prezime);
            queryDict.Add("telefon", korisnik.telefon);


            try
            {

                var query = new Neo4jClient.Cypher.CypherQuery("MATCH (n:Korisnik {ime : {ime}, prezime: {prezime}, telefon: {telefon}} ) SET n.telefon = {telefon} return n", queryDict, CypherResultMode.Set);

                ((IRawGraphClient)client).ExecuteCypher(query);

                txtIme.Text = "";
                txtPrezime.Text = "";
                txtTelefon.Text = "";
                MessageBox.Show("Uspešno ste izmenili iskustvo korisnika.");
            }

            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        private void txtIme_TextChanged(object sender, EventArgs e)
        {
            Korisnik korisnik = new Korisnik();

            korisnik.ime = txtIme.Text;
            Dictionary<string, object> queryDict = new Dictionary<string, object>();
            queryDict.Add("ime", korisnik.ime);

            var query = new Neo4jClient.Cypher.CypherQuery("MATCH (n:Korisnik {ime : {ime}} ) return n", queryDict, CypherResultMode.Set);
            List<Korisnik> korisnici = ((IRawGraphClient)client).ExecuteGetCypherResults<Korisnik>(query).ToList();

            foreach (Korisnik k in korisnici)
            {
                txtPrezime.Text =k.prezime;
                txtTelefon.Text = k.telefon;
            }

            groupBox1.Text = txtIme.Text;

        }

        private void btnPohadja_Click(object sender, EventArgs e)
        {
            if (txtIme.Text != "" && comboBox1.Text != "")
            {

                Korisnik korisnik = new Korisnik();
                Kurs kurs = new Kurs();

                korisnik.ime = txtIme.Text;
                kurs.id = comboBox1.Text;

                Dictionary<string, object> queryDict = new Dictionary<string, object>();
                queryDict.Add("ime", korisnik.ime);
                queryDict.Add("id", kurs.id);

                var query = new Neo4jClient.Cypher.CypherQuery("MATCH (a:Korisnik), (b:Kurs) WHERE a.ime = {ime} and b.id = {id} CREATE (a) - [r:POHADJA] -> (b)", queryDict, CypherResultMode.Set);

                ((IRawGraphClient)client).ExecuteCypher(query);

            }
            txtIme.Text = "";
            comboBox1.Text = "";
            groupBox1.Text = "";
        }
    }
}

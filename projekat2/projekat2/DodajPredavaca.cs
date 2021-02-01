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
    public partial class DodajPredavaca : Form
    {
        public GraphClient client;
        public DodajPredavaca()
        {
            InitializeComponent();
        }

        private void btnDodaj_Click(object sender, EventArgs e)
        {
            Predavac predavac = this.createPredavac();


            Dictionary<string, object> queryDict = new Dictionary<string, object>();
            queryDict.Add("ime", predavac.ime);
            queryDict.Add("prezime", predavac.prezime);
            queryDict.Add("obrazovanje", predavac.obrazovanje);
            queryDict.Add("iskustvo", predavac.iskustvo);


            
            var query = new Neo4jClient.Cypher.CypherQuery("CREATE (n:Predavac {ime : {ime}, prezime : {prezime}, obrazovanje : {obrazovanje}, iskustvo: {iskustvo} }) return n",
                                                           queryDict, CypherResultMode.Set);

            List<Predavac> predavaci = ((IRawGraphClient)client).ExecuteGetCypherResults<Predavac>(query).ToList();

            foreach (Predavac n in predavaci)
            {
                MessageBox.Show(n.ime);
            }


            this.Close();

        }

        private Predavac createPredavac()
        {
            Predavac n = new Predavac();

            n.ime = txtIme.Text;
            n.prezime = txtPrezime.Text;
            n.obrazovanje = txtObrazovanje.Text;
            n.iskustvo = txtIskustvo.Text;

            return n;
        }

        private void lblObrazovanje_Click(object sender, EventArgs e)
        {

        }

        private void DodajPredavaca_Load(object sender, EventArgs e)
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
        }

        private void btnPromeni_Click(object sender, EventArgs e)
        {
            Predavac predavac = new Predavac();

            predavac.ime = txtIme.Text;
            predavac.prezime = txtPrezime.Text;
            predavac.obrazovanje = txtObrazovanje.Text;
            predavac.iskustvo = txtIskustvo.Text;

            Dictionary<string, object> queryDict = new Dictionary<string, object>();
            queryDict.Add("ime", predavac.ime);
            queryDict.Add("prezime", predavac.prezime);
            queryDict.Add("obrazovanje", predavac.obrazovanje);
            queryDict.Add("iskustvo", predavac.iskustvo);

            try
            {

                var query = new Neo4jClient.Cypher.CypherQuery("MATCH (n:Predavac {ime : {ime}, prezime: {prezime}, obrazovanje: {obrazovanje}} ) SET n.iskustvo = {iskustvo} return n", queryDict, CypherResultMode.Set);

                ((IRawGraphClient)client).ExecuteCypher(query);

                txtIme.Text = "";
                txtPrezime.Text = "";
                txtObrazovanje.Text = "";
                txtIskustvo.Text = "";
                groupBox1.Text = "";
                MessageBox.Show("Uspešno ste izmenili iskustvo predavača.");
            }

            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        private void txtIme_TextChanged(object sender, EventArgs e)
        {
            Predavac predavac = new Predavac();

            predavac.ime = txtIme.Text;
            Dictionary<string, object> queryDict = new Dictionary<string, object>();
            queryDict.Add("ime", predavac.ime);

            var query = new Neo4jClient.Cypher.CypherQuery("MATCH (n:Predavac {ime : {ime}} ) return n", queryDict, CypherResultMode.Set);
            List<Predavac> predavaci = ((IRawGraphClient)client).ExecuteGetCypherResults<Predavac>(query).ToList();

            foreach (Predavac p in predavaci)
            {
                txtPrezime.Text = p.prezime;
                txtObrazovanje.Text = p.obrazovanje;
                txtIskustvo.Text = p.iskustvo;
            }

            groupBox1.Text = txtIme.Text;
        }

        private void btnPredaje_Click(object sender, EventArgs e)
        {
            if (txtIme.Text != "" && comboBox1.Text != "")
            {

                Predavac predavac = new Predavac();
                Kurs kurs = new Kurs();

                predavac.ime = txtIme.Text;
                kurs.id = comboBox1.Text;

                Dictionary<string, object> queryDict = new Dictionary<string, object>();
                queryDict.Add("ime", predavac.ime);
                queryDict.Add("id", kurs.id);

                var query = new Neo4jClient.Cypher.CypherQuery("MATCH (a:Kurs), (b:Predavac) WHERE a.id = {id} and b.ime= {ime} CREATE (a) - [r:PREDAJE] -> (b)", queryDict, CypherResultMode.Set);

                ((IRawGraphClient)client).ExecuteCypher(query);

            }
            txtIme.Text = "";
            txtPrezime.Text = "";
            txtObrazovanje.Text = "";
            txtIskustvo.Text = "";
            comboBox1.Text = "";
            groupBox1.Text = "";
        }
    }
}

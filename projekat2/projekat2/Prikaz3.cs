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

namespace projekat2
{
    public partial class Prikaz3 : Form
    {
        public Prikaz3()
        {
            InitializeComponent();
        }
        public Prikaz3(List<Korisnik> value)
        {
            InitializeComponent();
            this.Value = value;
        }
        public List<Korisnik> Value { get; set; }
        private void Prikaz3_Load(object sender, EventArgs e)
        {
            dataGridView1.DataSource = Value;
        }
    }
}

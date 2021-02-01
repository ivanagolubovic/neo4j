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
    public partial class Prikaz2 : Form
    {
        public Prikaz2()
        {
            InitializeComponent();
        }
        public Prikaz2(List<Predavac> value)
        {
            InitializeComponent();
            this.Value = value;
        }
        public List<Predavac> Value { get; set; }
        private void Prikaz2_Load(object sender, EventArgs e)
        {
            dataGridView1.DataSource = Value;
        }
    }
}

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
    public partial class Prikaz1 : Form
    {
        public Prikaz1()
        {
            InitializeComponent();
        }
        public Prikaz1(List<Skola> value)
        {
            InitializeComponent();
            this.Value = value;
        }
        public List<Skola> Value { get; set; }
        private void Prikaz1cs_Load(object sender, EventArgs e)
        {
            dataGridView1.DataSource = Value;
        }
    }
}

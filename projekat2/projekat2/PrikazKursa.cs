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
    public partial class PrikazKursa : Form
    {
        public PrikazKursa()
        {
            InitializeComponent();
        }

        public PrikazKursa(List<Kurs> value)
        {
            InitializeComponent();
            this.Value = value;
        }

        public List<Kurs> Value { get; set; }

        private void PrikazKursa_Load(object sender, EventArgs e)
        {
            dataGridView1.DataSource = Value;
        }
    }
}

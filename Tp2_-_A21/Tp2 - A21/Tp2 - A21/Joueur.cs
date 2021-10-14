using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tp2___A21
{
    public class Joueur
    {
        private List<Carte> _main;
        private string _nom;

        public string Nom
        {
            get { return _nom; }
            set { _nom = value; }
        }

        public List<Carte> Main
        {
            get { return _main; }
            set { _main = value; }
        }

        public Joueur(string pNom)
        {
            Main = new List<Carte>();
            Nom = pNom;
        }
    }
}

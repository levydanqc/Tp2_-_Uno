using System.Collections.Generic;

namespace Tp2___A21
{
    public class Joueur
    {
        private LinkedList<Carte> _main;
        private string _nom;
        private byte[] _mdp;

        private int _nbPige;

        public int NbPige
        {
            get { return _nbPige; }
            set { _nbPige = value; }
        }

        public string Nom
        {
            get { return _nom; }
            set { _nom = value; }
        }

        public LinkedList<Carte> Main
        {
            get { return _main; }
            set { _main = value; }
        }

        public byte[] Mdp
        {
            get { return _mdp; }
            set { _mdp = value; }
        }

        public Joueur(string pNom, byte[] pMdp)
        {
            Nom = pNom;
            Mdp = pMdp;
        }

        public Joueur(string pNom)
        {
            Main = new LinkedList<Carte>();
            Nom = pNom;
            _nbPige = 1;
        }

        public Joueur()
        {

        }
    }
}

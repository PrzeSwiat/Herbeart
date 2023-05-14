using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheGame
{
    internal class Inventory
    {
        private int appleLeafNumber;
        private int mintLeafNumber;
        private int melissaLeafNumber;
        private int nettleLeafNumber;

        public Inventory()
        {
            appleLeafNumber = 0;
            melissaLeafNumber = 0;
            mintLeafNumber = 0;
            nettleLeafNumber = 0;
        }

        public int[] returnLeafsList()
        {
            int[] leafs = new int[4];
            leafs[0] = nettleLeafNumber;
            leafs[1] = mintLeafNumber;
            leafs[2] = melissaLeafNumber;
            leafs[3] = appleLeafNumber;
            return leafs;
        }

        public int getAppleLeaftNumber()
        {
            return appleLeafNumber;
        }

        public int getMintLeafNumber()
        {
            return mintLeafNumber;
        }

        public int getMellisaLeafNumber()
        {
            return melissaLeafNumber;
        }

        public int getNettleLeafNumber()
        {
            return nettleLeafNumber;
        }

        public void addAppleLeaf()
        {
            appleLeafNumber++;
        }
        public void addMelissaLeaf()
        {
            melissaLeafNumber++;
        }
        public void addNettleLeaf()
        {
            nettleLeafNumber++;
        }
        public void addMintLeaf()
        {
            mintLeafNumber++;
        }
        public void removeAppleLeaf() { appleLeafNumber--; }
        public void removeMelissaLeaf() { melissaLeafNumber--; }
        public void removeNettleLeaf() { nettleLeafNumber--; }
        public void removeMintLeaf() { mintLeafNumber--; }

        public bool checkappleLeafNumber() { return appleLeafNumber > 0; }
        public bool checkMelissaLeafNumber() { return melissaLeafNumber > 0; }
        public bool checknettleLeafNumber() { return nettleLeafNumber > 0; }
        public bool checkmintLeafNumber() { return mintLeafNumber > 0; }
    }
}

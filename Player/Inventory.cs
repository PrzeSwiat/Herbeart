using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheGame
{
    internal class Inventory
    {
        public int appleLeafNumber;
        public int MintLeafNumber;
        public int MelissaLeafNumber;
        public int NettleLeafNumber;

        public Inventory()
        {
            appleLeafNumber = 0;
            MelissaLeafNumber = 0;
            MintLeafNumber = 0;
            NettleLeafNumber = 0;
        }

        public void addAppleLeaf()
        {
            appleLeafNumber++;
        }
        public void addMelissaLeaf()
        {
            MelissaLeafNumber++;
        }
        public void addNettleLeaf()
        {
            NettleLeafNumber++;
        }
        public void addMintLeaf()
        {
            MintLeafNumber++;
        }
        public void removeAppleLeaf() { appleLeafNumber--; }
        public void removeMelissaLeaf() { MelissaLeafNumber--; }
        public void removeNettleLeaf() { NettleLeafNumber--; }
        public void removeMintLeaf() { MintLeafNumber--; }

        public bool checkappleLeafNumber() { return appleLeafNumber > 0; }
        public bool checkMelissaLeafNumber() { return MelissaLeafNumber > 0; }
        public bool checknettleLeafNumber() { return NettleLeafNumber > 0; }
        public bool checkmintLeafNumber() { return MintLeafNumber > 0; }
    }
}

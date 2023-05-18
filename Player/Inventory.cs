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
        private int meliseLeafNumber;
        private int nettleLeafNumber;

        public Inventory()
        {
            appleLeafNumber = 3;
            meliseLeafNumber = 3;
            mintLeafNumber = 3;
            nettleLeafNumber = 3;
        }

        public Dictionary<string, int> returnLeafs()
        {
            Dictionary<string, int> leafs = new Dictionary<string, int>();
            leafs.Add("A", mintLeafNumber);
            leafs.Add("B", nettleLeafNumber);
            leafs.Add("X", meliseLeafNumber);
            leafs.Add("Y", appleLeafNumber);
            
            return leafs;
        }
        #region Getters
        public int getAppleLeaftNumber()
        {
            return appleLeafNumber;
        }

        public int getMintLeafNumber()
        {
            return mintLeafNumber;
        }

        public int getMeliseLeafNumber()
        {
            return meliseLeafNumber;
        }

        public int getNettleLeafNumber()
        {
            return nettleLeafNumber;
        }
        #endregion
        #region AddANDRemove
        public void addAppleLeaf()
        {
            appleLeafNumber++;
        }
        public void addMeliseLeaf()
        {
            meliseLeafNumber++;
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
        public void removeMeliseLeaf() { meliseLeafNumber--; }
        public void removeNettleLeaf() { nettleLeafNumber--; }
        public void removeMintLeaf() { mintLeafNumber--; }
        #endregion
        public bool checkAppleLeafNumber() { return appleLeafNumber > 0; }
        public bool checkMeliseLeafNumber() { return meliseLeafNumber > 0; }
        public bool checkNettleLeafNumber() { return nettleLeafNumber > 0; }
        public bool checkMintLeafNumber() { return mintLeafNumber > 0; }
    }
}

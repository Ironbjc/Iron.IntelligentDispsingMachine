using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iron.IntelligentDispsingMachine.Models
{
    /// <summary>
    /// 存放位置010125  010201  
    /// </summary>
    public class StorageModel
    {
        public int CabID { get; set; } //未用第几列设备
        public int PValue { get; set; }//实际位置
        public int Colum { get; set; }
        public int Row { get; set; }
           // public int MotorID { get; set; }//电机的列

        public LedModel Led { get; set; }
        public MotorPositon motorPositon { get; set; } = new MotorPositon();
        public static StorageModel GetStorageByPositon(string position)
        {
            StorageModel storageModel = new StorageModel();
            if(!string.IsNullOrEmpty(position))
            {
                var lie = position.Substring(2, 2);
                var result = position.Substring(4, 2);
                var count = int.Parse(result);
                if (new string[] {"01","02"}.Contains(lie))
                {
                    storageModel.motorPositon.direction = Direction.Left;
                    if(lie=="01")
                    {                   
                        if(new int[] {1,2,3,4,5,6,7,8,9}.Contains(count))
                        {
                            storageModel.motorPositon.rowPosition = RowPosition.First;
                        }
                        else if(new int[] {10,11,12,13,14,15,16,17,18}.Contains(count))
                        {
                            storageModel.motorPositon.rowPosition = RowPosition.Middle;
                        }
                        else if(new int[] {19,20,21,22,23,24,25,26,27}.Contains(count))
                        {
                            storageModel.motorPositon.rowPosition = RowPosition.Third;
                        }
                    }
                    else if(lie=="02")
                    {
                        if (new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 }.Contains(count))
                        {
                            storageModel.motorPositon.rowPosition = RowPosition.Third;
                        }
                        else if (new int[] { 10, 11, 12, 13, 14, 15, 16, 17, 18 }.Contains(count))
                        {
                            storageModel.motorPositon.rowPosition = RowPosition.Middle;
                        }
                        else if (new int[] { 19, 20, 21, 22, 23, 24, 25, 26, 27 }.Contains(count))
                        {
                            storageModel.motorPositon.rowPosition = RowPosition.First;
                        }
                    }
                }
                else if(new string[] {"03","04"}.Contains(lie))
                {
                    storageModel.motorPositon.direction = Direction.Right;
                    if (lie == "03")
                    {
                        if (new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 }.Contains(count))
                        {
                            storageModel.motorPositon.rowPosition = RowPosition.Third;
                        }
                        else if (new int[] { 10, 11, 12, 13, 14, 15, 16, 17, 18 }.Contains(count))
                        {
                            storageModel.motorPositon.rowPosition = RowPosition.Middle;
                        }
                        else if (new int[] { 19, 20, 21, 22, 23, 24, 25, 26, 27 }.Contains(count))
                        {
                            storageModel.motorPositon.rowPosition = RowPosition.First;
                        }
                    }
                    else if (lie == "04")
                    {
                        if (new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 }.Contains(count))
                        {
                            storageModel.motorPositon.rowPosition = RowPosition.First;
                        }
                        else if (new int[] { 10, 11, 12, 13, 14, 15, 16, 17, 18 }.Contains(count))
                        {
                            storageModel.motorPositon.rowPosition = RowPosition.Middle;
                        }
                        else if (new int[] { 19, 20, 21, 22, 23, 24, 25, 26, 27 }.Contains(count))
                        {
                            storageModel.motorPositon.rowPosition = RowPosition.Third;
                        }
                    }
                }
            }
            return storageModel;
        }
        
    }

    
}

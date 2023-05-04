using Iron.IntelligentDispsingMachine.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iron.IntelligentDispsingMachine.Models
{
    /// <summary>
    /// 每一个存储位，需要灯，电机，药品
    /// </summary>
    public class StorageCoreModel
    {
        public string MedPos { get; set; }
        public LedModel Led { get; set; }
        public MotorPositon motor { get; set; }
       

        public StorageMedModel Med { get; set; }


        public StorageCoreModel(string medPos)
        {
            this.MedPos = medPos;
            this.Led = GetLed(medPos);
            this.motor = GetMoto(medPos);
            this.Med = GetMed(medPos);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public LedModel GetLed(string medPos)
        {
            var lie = medPos.Substring(2, 2);
            var result = medPos.Substring(4, 2);
            var rowResult = int.Parse(result) % 9;
            int row;
            int X = 0;
            int Y = 0;
            if (new int[] { 0, 3, 6 }.Contains(rowResult))
                X = (int.Parse(lie) - 1) * 3 + 3;

            if (new int[] { 1, 4, 7 }.Contains(rowResult))
                X = (int.Parse(lie) - 1) * 3 + 1;
            if (new int[] { 2, 5, 8 }.Contains(rowResult))
                X = (int.Parse(lie) - 1) * 3 + 2;

            if (new int[] { 1, 2, 3 }.Contains(rowResult))
                Y = 1;
            if (new int[] { 4, 5, 6 }.Contains(rowResult))
                Y = 2;
            if (new int[] { 7, 8, 0 }.Contains(rowResult))
                Y = 3;
            LedModel led = new LedModel()
            {
                X = X,
                Y = Y,
                IsLight = false
            };
            return led;
        }

        /// <summary>
        /// 从数据库根据位置获取药品信息
        /// </summary>
        /// <param name="medPos"></param>
        /// <returns></returns>
        public StorageMedModel GetMed(string medPos)
        {
            try
            {
                StorageMedModel storageMedModel = new StorageMedModel();
                //从数据库获取
                var result = GlobalValue.LocalDataAccess.GetMedInStoreByMedPos(medPos, GlobalValue.MachineID);
                if (result != null)
                {
                    var r1 = result.Select(p => new StorageMedModel()
                    {
                        MedFactory = p.MedFactory,
                        MedName = p.MedName,
                        MedNowAMT = p.MedNowAMT,
                        MedOnlyCode = p.MedOnlyCode,
                        MedPack = p.MedPack,
                        MedPos = p.MedPos,
                        MedPYCode = p.MedPYCode,
                        MedUnit = p.MedUnit,
                        MedValidTime = p.MedValidTime,
                    }).FirstOrDefault();
                    if (r1 != null)
                        storageMedModel = r1;
                }



                return storageMedModel;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        
    
            
        }
        public MotorPositon GetMoto(string position)
        {
            MotorPositon m = new MotorPositon();
            if (!string.IsNullOrEmpty(position))
            {
                var lie = position.Substring(2, 2);
                var result = position.Substring(4, 2);
                var count = int.Parse(result);
                if (new string[] { "01", "02" }.Contains(lie))
                {
                    m.direction = Direction.Left;
                    if (lie == "01")
                    {
                        if (new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 }.Contains(count))
                        {
                            m.rowPosition = RowPosition.First;
                        }
                        else if (new int[] { 10, 11, 12, 13, 14, 15, 16, 17, 18 }.Contains(count))
                        {
                            m.rowPosition = RowPosition.Middle;
                        }
                        else if (new int[] { 19, 20, 21, 22, 23, 24, 25, 26, 27 }.Contains(count))
                        {
                            m.rowPosition = RowPosition.Third;
                        }
                    }
                    else if (lie == "02")
                    {
                        if (new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 }.Contains(count))
                        {
                            m.rowPosition = RowPosition.Third;
                        }
                        else if (new int[] { 10, 11, 12, 13, 14, 15, 16, 17, 18 }.Contains(count))
                        {
                            m.rowPosition = RowPosition.Middle;
                        }
                        else if (new int[] { 19, 20, 21, 22, 23, 24, 25, 26, 27 }.Contains(count))
                        {
                            m.rowPosition = RowPosition.First;
                        }
                    }
                }
                else if (new string[] { "03", "04" }.Contains(lie))
                {
                    m.direction = Direction.Right;
                    if (lie == "03")
                    {
                        if (new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 }.Contains(count))
                        {
                            m.rowPosition = RowPosition.First;
                        }
                        else if (new int[] { 10, 11, 12, 13, 14, 15, 16, 17, 18 }.Contains(count))
                        {
                            m.rowPosition = RowPosition.Middle;
                        }
                        else if (new int[] { 19, 20, 21, 22, 23, 24, 25, 26, 27 }.Contains(count))
                        {
                            m.rowPosition = RowPosition.Third;
                        }
                    }
                    else if (lie == "04")
                    {
                        if (new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 }.Contains(count))
                        {
                            m.rowPosition = RowPosition.Third;
                        }
                        else if (new int[] { 10, 11, 12, 13, 14, 15, 16, 17, 18 }.Contains(count))
                        {
                            m.rowPosition = RowPosition.Middle;
                        }
                        else if (new int[] { 19, 20, 21, 22, 23, 24, 25, 26, 27 }.Contains(count))
                        {
                            m.rowPosition = RowPosition.First;
                        }
                    }
                }
            }
            return m;
        }
    }
}

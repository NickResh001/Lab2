using Lab2.DTOs;
using System.Diagnostics;

namespace Lab2.Models
{
    public struct HeatingModelSettings
    {
        #region Границы
        // Границы задаются линейной функцией от двух аргументов - двух координат.
        // При желании можно занулить веса для координат и получится граница заданная константой
        public double A_i_weight = 0;   // Граница А, вес для координаты i
        public double A_j_weight = 0;   // Граница А, вес для координаты j
        public double A_free_weight = 100;// Граница А, свободный член
        // AA - границы в тех же координатах, что и А, только на другой стороне куба
        public double AA_i_weight = 0;
        public double AA_j_weight = 0;
        public double AA_free_weight = 100;

        public double B_k_weight = 0;
        public double B_j_weight = 0;
        public double B_free_weight = 100;

        public double BB_k_weight = 0;
        public double BB_j_weight = 0;
        public double BB_free_weight = 100;

        public double C_i_weight = 0;
        public double C_k_weight = 0;
        public double C_free_weight = 100;

        public double CC_i_weight = 0;
        public double CC_k_weight = 0;
        public double CC_free_weight = 100;
        #endregion

        // Размеры параллелепипеда в см
        public double iActualSize = 1;
        public double jActualSize = 1;
        public double kActualSize = 1;

        // Шаги и коэффициент
        public double h = 0.05;
        public double tau = 0.01;
        public double a = 0.03;

        public double initTime = 0;
        public double maxTime = 1;

        // Условие устойчивости
        public bool isStable => tau * a * a / (h * h) < 0.125;

        // Массивы температур
        public double[][][] u;
        public double[][][] uNew;

        public double execTime = 0;
        // размеры массивов
        public int iSize => (int)(iActualSize / h);
        public int jSize => (int)(jActualSize / h);
        public int kSize => (int)(kActualSize / h);

        public HeatingModelSettings() { }

        public HeatingModelSettings(SettingsDto settingsDto)
        {
            A_i_weight = settingsDto.A_i_weight;
            A_j_weight = settingsDto.A_j_weight;
            A_free_weight = settingsDto.A_free_weight;
            AA_i_weight = settingsDto.AA_i_weight;
            AA_j_weight = settingsDto.AA_j_weight;
            AA_free_weight = settingsDto.AA_free_weight;
            B_k_weight = settingsDto.B_k_weight;
            B_j_weight = settingsDto.B_j_weight;
            B_free_weight = settingsDto.B_free_weight;
            BB_k_weight = settingsDto.BB_k_weight;
            BB_j_weight = settingsDto.BB_j_weight;
            BB_free_weight = settingsDto.BB_free_weight;
            C_i_weight = settingsDto.C_i_weight;
            C_k_weight = settingsDto.C_k_weight;
            C_free_weight = settingsDto.C_free_weight;
            CC_i_weight = settingsDto.CC_i_weight;
            CC_k_weight = settingsDto.CC_k_weight;
            CC_free_weight = settingsDto.CC_free_weight;

            iActualSize = settingsDto.iActualSize; 
            jActualSize = settingsDto.jActualSize; 
            kActualSize = settingsDto.kActualSize; 
            h = settingsDto.h; 
            tau = settingsDto.tau; 
            a = settingsDto.a; 

            initTime = settingsDto.initTime;
            maxTime = settingsDto.maxTime;
    }
    }
    public class HeatingModeling3d
    {
        public HeatingModelSettings st;
        public int execTime;

        public HeatingModeling3d()
        {
        }

        private void CopyArray(double[][][] src, double[][][] dst)
        {
            for (int i = 0; i < st.iSize; i++)
                for (int j = 0; j < st.jSize; j++)
                    for (int k = 0; k < st.kSize; k++)
                        dst[i][j][k] = src[i][j][k];
        }
        private void InitializeU()
        {
            st.u = new double[st.iSize][][];
            st.uNew = new double[st.iSize][][];
            int iSizeVar = st.iSize;
            int jSizeVar = st.jSize;
            int kSizeVar = st.kSize;

            Parallel.For(0, iSizeVar, i =>
            {
                st.u[i] = new double[jSizeVar][];
                st.uNew[i] = new double[jSizeVar][];
                for (int j = 0; j < jSizeVar; j++)
                {
                    st.u[i][j] = new double[kSizeVar];
                    st.uNew[i][j] = new double[kSizeVar];
                    for (int k = 0; k < kSizeVar; k++)
                    {
                        st.u[i][j][k] = 0;
                        st.uNew[i][j][k] = 0;
                    }
                }
            });

        }
        private void FillBoundaries()
        {
            Parallel.For(0, st.iSize, i =>
            {
                for (int j = 0; j < st.jSize; j++)
                {
                    st.u[i][j][0] = st.A_i_weight * i + st.A_j_weight * j + st.A_free_weight;
                    st.u[i][j][st.kSize - 1] = st.AA_i_weight * i + st.AA_j_weight * j + st.AA_free_weight;
                }
            });
            Parallel.For(0, st.jSize, j =>
            {
                for (int k = 0; k < st.kSize; k++)
                {
                    st.u[0][j][k] = st.B_j_weight * j + st.B_k_weight * k + st.B_free_weight;
                    st.u[st.iSize - 1][j][k] = st.BB_j_weight * j + st.BB_k_weight * k + st.BB_free_weight;
                }
            });
            Parallel.For(0, st.iSize, i =>
            {
                for (int k = 0; k < st.kSize; k++)
                {
                    st.u[i][0][k] = st.C_i_weight * i + st.C_k_weight * k + st.C_free_weight;
                    st.u[i][st.jSize - 1][k] = st.CC_i_weight * i + st.CC_k_weight * k + st.CC_free_weight;
                }
            });
            CopyArray(st.u, st.uNew);
        }
        private double Formula(int i, int j, int k)
        {
            return st.u[i][j][k] + st.tau * st.a * st.a / (st.h * st.h) *
                        (st.u[i - 1][j][k] + st.u[i + 1][j][k] +
                        st.u[i][j - 1][k] + st.u[i][j + 1][k] +
                        st.u[i][j][k - 1] + st.u[i][j][k + 1] -
                        6 * st.u[i][j][k]);
        }

        public (double min, double max) GetMinAndMax()
        {
            double min = double.MaxValue;
            double max = double.MinValue;

            for (int i = 0; i < st.iSize; i++)
            {
                for (int j = 0; j < st.jSize; j++)
                {
                    max = Math.Max(st.uNew[i][j].Max(), max);
                    min = Math.Min(st.uNew[i][j].Min(), min);
                }
            }

            return (min, max);
        }
        public void SolveParallel(HeatingModelSettings settings)
        {
            st = settings;
            if (!st.isStable)
                return;

            InitializeU();
            FillBoundaries();

            Stopwatch timer = new();
            timer.Start();

            // Цикл по времени
            double time = st.initTime;
            while (time < st.maxTime)
            {
                Parallel.For(1, st.iSize - 1, i =>
                {
                    for (int j = 1; j < st.jSize - 1; j++)
                    {
                        for (int k = 1; k < st.kSize - 1; k++)
                        {
                            st.uNew[i][j][k] = Formula(i, j, k);
                        }
                    }
                });
                CopyArray(st.uNew, st.u);
                time += st.tau;
            }

            timer.Stop();
            st.execTime = timer.Elapsed.TotalMilliseconds / 1000;
        }
        public void Solve(HeatingModelSettings settings)
        {
            st = settings;
            if (!st.isStable)
                return;

            InitializeU();
            FillBoundaries();

            Stopwatch timer = new();
            timer.Start();

            // Цикл по времени
            double time = st.initTime;
            while (time < st.maxTime)
            {
                for (int i = 1; i < st.iSize - 1; i++)
                {
                    for (int j = 1; j < st.jSize - 1; j++)
                    {
                        for (int k = 1; k < st.kSize - 1; k++)
                        {
                            st.uNew[i][j][k] = Formula(i, j, k);
                        }
                    }
                }
                CopyArray(st.uNew, st.u);
                time += st.tau;
            }

            timer.Stop();
            st.execTime = timer.Elapsed.TotalMilliseconds / 1000;
        }

    }
}

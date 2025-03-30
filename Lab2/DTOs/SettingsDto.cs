using Lab2.Services;

namespace Lab2.DTOs
{
    public class SettingsDto
    {
        public bool IsParallel { get; set; }

        public double A_i_weight { get; set; }
        public double A_j_weight { get; set; }
        public double A_free_weight { get; set; }
        public double AA_i_weight { get; set; }
        public double AA_j_weight { get; set; }
        public double AA_free_weight { get; set; }
        public double B_k_weight { get; set; }
        public double B_j_weight { get; set; }
        public double B_free_weight { get; set; }
        public double BB_k_weight { get; set; }
        public double BB_j_weight { get; set; }
        public double BB_free_weight { get; set; }
        public double C_i_weight { get; set; }
        public double C_k_weight { get; set; }
        public double C_free_weight { get; set; }
        public double CC_i_weight { get; set; }
        public double CC_k_weight { get; set; }
        public double CC_free_weight { get; set; }

        public double iActualSize { get; set; }
        public double jActualSize { get; set; }
        public double kActualSize { get; set; }

        public double h { get; set; }
        public double tau { get; set; }
        public double a { get; set; }

        public double initTime { get; set; }
        public double maxTime { get; set; }

        //public SettingsDto(SettingsDto settings)
        //{
        //    A_i_weight = settings.A_i_weight;
        //    A_j_weight = settings.A_j_weight;
        //    A_free_weight = settings.A_free_weight;
        //    AA_i_weight = settings.AA_i_weight;
        //    AA_j_weight = settings.AA_j_weight;
        //    AA_free_weight = settings.AA_free_weight;
        //    B_k_weight = settings.B_k_weight;
        //    B_j_weight = settings.B_j_weight;
        //    B_free_weight = settings.B_free_weight;
        //    BB_k_weight = settings.BB_k_weight;
        //    BB_j_weight = settings.BB_j_weight;
        //    BB_free_weight = settings.BB_free_weight;
        //    C_i_weight = settings.C_i_weight;
        //    C_k_weight = settings.C_k_weight;
        //    C_free_weight = settings.C_free_weight;
        //    CC_i_weight = settings.CC_i_weight;
        //    CC_k_weight = settings.CC_k_weight;
        //    CC_free_weight = settings.CC_free_weight;

        //    iActualSize = settings.iActualSize;
        //    jActualSize = settings.jActualSize;
        //    kActualSize = settings.kActualSize;
        //    h = settings.h;
        //    tau = settings.tau;
        //    a = settings.a;
        //    initTime = settings.initTime;
        //    maxTime = settings.maxTime;
        //    execTime = settings.execTime;

        //    isParallel = settings.IsParallel;
        //}
    }
}

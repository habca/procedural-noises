using System;
using System.Numerics;

namespace Perlin
{
    public static class ImprovedNoiseDerivatives
    {
        static public Vector4 noisedBitman(double x, double y, double z) {
            int X = (int)Math.Floor(x) & 255;                  // FIND UNIT CUBE THAT
            int Y = (int)Math.Floor(y) & 255;                  // CONTAINS POINT.
            int Z = (int)Math.Floor(z) & 255;
            
            x -= Math.Floor(x);                                // FIND RELATIVE X,Y,Z
            y -= Math.Floor(y);                                // OF POINT IN CUBE.
            z -= Math.Floor(z);
            
            double u = fade(x);                                // COMPUTE FADE CURVES
            double v = fade(y);                                // FOR EACH OF X,Y,Z.
            double w = fade(z);

            double du = derva(x);
            double dv = derva(y);
            double dw = derva(z);
            
            int A = p[X  ]+Y, AA = p[A]+Z, AB = p[A+1]+Z;      // HASH COORDINATES OF
            int B = p[X+1]+Y, BA = p[B]+Z, BB = p[B+1]+Z;      // THE 8 CUBE CORNERS,

            double va = grad(p[AA  ], x  , y  , z   ),
                   vb = grad(p[BA  ], x-1, y  , z   ),
                   vc = grad(p[AB  ], x  , y-1, z   ),
                   ve = grad(p[AA+1], x  , y  , z-1 ),
                   vf = grad(p[BA+1], x-1, y  , z-1 ),
                   vg = grad(p[AB+1], x  , y-1, z-1 ),
                   vd = grad(p[BB  ], x-1, y-1, z   ),
                   vh = grad(p[BB+1], x-1, y-1, z-1 );


            double val = va * (1 - u) * (1 - v) * (1 - w) +
                         vb * u * (1 - v) * (1 - w) +
                         vc * (1 - u) * v * (1 - w) +
                         ve * (1 - u) * (1 - v) * w +
                         vf * u * (1 - v) * w +
                         vg * (1 - u) * v * w +
                         vd * u * v * (1 - w) +
                         vh * u * v * w;

            double dx = du * (vb * (1 - v) * (1 - w) + 
                              vf * (1 - v) * w + 
                              vd * v * (1 - w) + 
                              vh * v * w);
            
            double dy = dv * (vc * (1 - u) * (1 - w) +
                              vg * (1 - u) * w +
                              vd * u * (1 - w) +
                              vh * u * w);

            double dz = dw * (ve * (1 - u) * (1 - v) +
                              vf * u * (1 - v) +
                              vg * (1 - u) * v +
                              vh * u * v);

            return new Vector4(
                (float)val,
                (float)dx,
                (float)dy,
                (float)dz);
        }

        static double fade(double t)
        { 
            return t * t * t * (t * (t * 6 - 15) + 10);
        }

        static double derva(double t)
        {
            return 30 * t * t * (t * (t - 2) + 1);
        }

        static double lerp(double t, double a, double b)
        {
            return a + t * (b - a);
        }

        static double grad(int hash, double x, double y, double z) {
            int h = hash & 15;
            double u = h<8 ? x : y,
                    v = h<4 ? y : h==12||h==14 ? x : z;
            return ((h&1) == 0 ? u : -u) + ((h&2) == 0 ? v : -v);
        }

        public static int[] p = new int[512];
        public static int[] permutation = { 151,160,137,91,90,15,
            131,13,201,95,96,53,194,233,7,225,140,36,103,30,69,142,8,99,37,240,21,10,23,
            190, 6,148,247,120,234,75,0,26,197,62,94,252,219,203,117,35,11,32,57,177,33,
            88,237,149,56,87,174,20,125,136,171,168, 68,175,74,165,71,134,139,48,27,166,
            77,146,158,231,83,111,229,122,60,211,133,230,220,105,92,41,55,46,245,40,244,
            102,143,54, 65,25,63,161, 1,216,80,73,209,76,132,187,208, 89,18,169,200,196,
            135,130,116,188,159,86,164,100,109,198,173,186, 3,64,52,217,226,250,124,123,
            5,202,38,147,118,126,255,82,85,212,207,206,59,227,47,16,58,17,182,189,28,42,
            223,183,170,213,119,248,152, 2,44,154,163, 70,221,153,101,155,167, 43,172,9,
            129,22,39,253, 19,98,108,110,79,113,224,232,178,185, 112,104,218,246,97,228,
            251,34,242,193,238,210,144,12,191,179,162,241, 81,51,145,235,249,14,239,107,
            49,192,214, 31,181,199,106,157,184, 84,204,176,115,121,50,45,127, 4,150,254,
            138,236,205,93,222,114,67,29,24,72,243,141,128,195,78,66,215,61,156,180
        };

        static ImprovedNoiseDerivatives()
        { 
            for (int i=0; i < 256; i++)
            {
                p[256+i] = p[i] = permutation[i];
            }
        }
    }
}
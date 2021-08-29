using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeaponTester
{
    class Weapon
    {
        int distance;
        int accuracy;
        int maxPenetration;
        int wounds;
        public bool power { get; set; } //Мощь
        public bool optic { get; set; }  //Оптика
        public bool up { get; set; }  //Подъем
        public bool pool { get; set; }  //Пул
        public bool auto { get; set; }  //Автомат
        public Weapon(int dist, int acc, int maxPen, int wounds)
        {
            this.distance = dist;
            this.accuracy = acc;
            this.maxPenetration = maxPen;
            this.wounds = wounds;
            this.power = false;
            this.optic = false;
            this.up = false;
            this.pool = false;
            this.auto = false;
        }

        public double Shoot(int skill, int armor, int dist)
        {
            int mpBonus = 0;
            int skillBonus = 0;
            Random random = new Random();

            if (dist <= 3)
            {
                skillBonus += 3;
                mpBonus += 1;
            }
            else if (dist <= distance)
            {
            }
            else if (dist <= 2 * distance)
            {
                mpBonus -= 1;
                skillBonus -= 3;
            }
            else if (dist <= 3 * distance)
            {
                mpBonus -= 2;
                skillBonus -= 6;
            }
            else if (dist <= 10 * distance)
            {
                mpBonus -= 3;
                skillBonus -= 9;
            }
            if (dist >= 30 && optic)
            {
                skillBonus += 6;
            }
            if (auto && dist >= 12)
            {
                skillBonus -= 6;
            }
            skill += skillBonus;
            skill += accuracy;
            maxPenetration += mpBonus;
            if (skill<0)
            {
                skill = 0;
            }
            double averageDamage = 0;
            double[] probSucc = new double[4];
            if (skill >= 5)  // вероятность 1 категории успеха
            {
                probSucc[1] = 0.25;
            }
            else
            {
                probSucc[1] = skill / 20.0;
            }

            if (skill >= 10) // вероятность 2 категории успеха
            {
                probSucc[2] = 0.25;
            }
            else
            {
                if (skill > 5)
                {
                    probSucc[2] = (skill - 5) / 20.0;
                }
                else
                {
                    probSucc[2] = 0;
                }

            }

            if (skill >= 20) // вероятность 3 категории успеха
            {
                probSucc[3] = 0.5;
            }
            else
            {
                if (skill > 10)
                {
                    probSucc[3] = (skill - 10) / 20.0;
                }
                else
                {
                    probSucc[3] = 0;
                }
            }

            if (skill < 12)
            {
                probSucc[3] += 0.05;
            }
            else if (skill >= 13)
            {
                probSucc[3] -= 0.05;
            }

            probSucc[0] = 1 - probSucc[1] - probSucc[2] - probSucc[3]; // вероятность провала
            double[] probHit;

            if (this.auto)
            {
                if (skill >= 12)
                {
                    probHit = new double[skill + 1];
                }
                else
                {
                    probHit = new double[13];
                    probHit[12] = 0.05;
                }
                
                
                for (int j = 1; j <= skill; j++)
                {
                    probHit[j] = 0.05;
                }
                
            }
            else
            {
                probHit = new double[4];
                if (pool || up)
                {
                    probHit[1] = 1 - probSucc[0];
                    probHit[2] = 0;
                    probHit[3] = 0;
                }
                else
                {
                    for (int j = 0; j < 4; j++)
                    {
                        probHit[j] = probSucc[j];
                    }
                }
            }

            double probPen = 1;
            if (this.pool)
            {
                probPen = (maxPenetration - armor) / 12.0;
                if (probPen < 1)
                {
                    probPen += 1 / 12.0;
                }
                probPen = (1 - (1 - probPen) * probSucc[1] + (1 - probPen) * (1 - probPen) * probSucc[2] + (1 - probPen) * (1 - probPen) * (1 - probPen) * probSucc[3]);
            }
            else
            {
                probPen = (maxPenetration - armor) / 12.0;
                if (probPen < 1)
                {
                    probPen += 1 / 12.0;
                }
            }
            probHit[0] = 1;
            for (int j = 1; j < probHit.Length; j++)
            {
                double tempProbHit = probHit[j];
                for (int l = j; l < probHit.Length; l++)
                {
                    double tempProb = 1;
                    for (int k = 0; k < j; k++)
                    {
                        tempProb *= probPen;
                    }
                    for (int k = 0; k < l - j; k++)
                    {
                        tempProb *= 1 - probPen;
                    }
                    tempProbHit += probHit[l] * tempProb;
                }
                probHit[j] = tempProbHit;
                probHit[0] -= probHit[j];
            }

            double damageForOneAttack = 0;
            if (this.power)
            {
                for (int j = 1; j <= maxPenetration - armor; j++)
                {
                    damageForOneAttack += j * 1 / 12.0;
                }
            }
            else if (up)
            {
                for (int j = 1; j <= 3; j++)
                {
                    damageForOneAttack += j * probSucc[j];
                }
            }
            else
            {
                damageForOneAttack += this.wounds;
            }
            for (int i = 1; i < probHit.Length; i++)
            {
                averageDamage += probHit[i] * i * damageForOneAttack;
            }
            return averageDamage;
        }
    }
}

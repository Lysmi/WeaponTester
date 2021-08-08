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
        int Trauma(int damage)
        {
            int sumDamage = damage;
            Random rand = new Random();
            int res = rand.Next(20) + 1;
            switch (res)
            {
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                    sumDamage += 1;
                    break;
                case 10:
                    if (sumDamage >= 3)
                    {
                        sumDamage += 20;
                    }
                    sumDamage += 4;
                    break;
                case 11:
                    if (sumDamage >= 3)
                    {
                        sumDamage += 2;
                    }
                    sumDamage += 3;
                    break;
                case 12:
                    break;
                case 13:
                    sumDamage += 3;
                    break;
                case 14:
                    sumDamage += 1;
                    break;
                case 15:
                    if (sumDamage >= 3)
                    {
                        sumDamage += 1;
                    }
                    sumDamage += 3;
                    break;
                case 16:
                    if (sumDamage >= 3)
                    {
                        sumDamage += 1;
                    }
                    sumDamage += 2;
                    break;
                case 17:
                    if (sumDamage >= 3)
                    {
                        sumDamage += 2;
                    }
                    sumDamage += 1;
                    break;
                case 18:
                    if (sumDamage >= 3)
                    {
                        sumDamage += 3;
                    }
                    break;
                case 19:
                    if (sumDamage >= 3)
                    {
                        sumDamage += 1;
                    }
                    sumDamage += 1;
                    break;
                case 20:
                    sumDamage += 1;
                    break;
                default:
                    break;
            }
            return sumDamage;
        }

        int Hit(int sucCount, int penVal)
        {
            int damage = 0;            
            if (this.power)
            {
                damage += penVal;
            }
            else if (up)
            {
                damage += sucCount;
            }
            else
            {
                damage += this.wounds;
            }
            if (penVal > 5)
            {
                damage += Trauma(damage);
            }
            return damage;      
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
            else if (dist <= 2*distance)
            {
                mpBonus -= 1;
                skillBonus -= 3;
            }
            else if (dist <= 3*distance)
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
            if (auto)
            {
                skillBonus -= 6;
            }
            skill += skillBonus;
            skill += accuracy;
            maxPenetration += mpBonus;
            double averageDamage = 0;
            for (int i = 0; i < 500000; i++)
            {
                int succ = 0;
                int res = random.Next(20) + 1;
                if ((res<=skill || res == 12) && res != 13)
                {
                    if (res <= 5)
                    {
                        succ = 1;
                    }
                    else if (res <= 10)
                    {
                        succ = 2;
                    }
                    else
                    {
                        succ = 3;
                    }
                }
                int countHit = succ;
                if (this.auto && succ > 0)
                {
                    countHit = res;
                }
                if (pool || up && succ > 0)
                {
                    countHit = 1;
                }
                for (int j = 0; j < countHit; j++)
                {
                    int resPen = 0;
                    if (this.pool)
                    {
                        for (int l = 0; l < succ; l++)
                        {
                            int curPen = random.Next(12) + 1;
                            if (curPen > resPen)
                            {
                                resPen = curPen;
                            }
                        }
                    }
                    else
                    {
                        resPen = random.Next(12) + 1;
                    }
                    if (resPen <= maxPenetration-armor)
                    {
                        averageDamage += Hit(succ, resPen);
                    }
                }
            }
            averageDamage /= 500000;
            return averageDamage;
        }
    }
}

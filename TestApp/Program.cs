using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestApp
{
    public class Program
    {
        static void Main(string[] args)
        {
            List<Menu> p = new List<Menu>() { new Menu() { name = "1", listMenu = new List<Menu>() { new Menu() { name = "11" }, new Menu() { name = "12" } } }, new Menu() { name = "2", listMenu = new List<Menu>() { new Menu() { name = "21",listMenu= new List<Menu>() { new Menu() { name = "211" }, new Menu() { name = "212" } } }, new Menu() { name = "22", isdelete = 1 ,listMenu= new List<Menu>() { new Menu() { name = "221" }, new Menu() { name = "222" } } } } } };
            Program program= new Program();
            var result= program.sss(p);
            program.show(result);
        }
        public List<Menu> sss(List<Menu> m)
        {
            if(m==null)
            {
                return null;
            }
            else
            {
                for (int i = m.Count-1; i >=0; i--)
                {
                    if(m[i].isdelete == 1)
                    {
                        m.Remove(m[i]);
                    }
                    else
                    {
                        sss(m[i].listMenu);
                    }
                }
                return m;
            }
        }
        public void show(List<Menu> p)
        {
            if(p==null)
            {
                return;
            }
            else
            {
                for (int i = 0; i < p.Count; i++)
                {
                    Console.WriteLine(p[i].name);
                    show(p[i].listMenu);
                }
            }
        }
    }
    
    public class Menu
    {
        public string name { get; set; }
        public List<Menu> listMenu {  get; set; }
        public int isdelete { get; set; }
        public int isroot {  get; set; }
    }
}

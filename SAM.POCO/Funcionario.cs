using System;

namespace SAM.POCO
{
    public class Funcionario
    {
        public int IdFuncionario { get; set; }
        public string Telefone { get; set; }
        public string Celular { get; set; }
        public string Foto { get; set; }
        public string Matricula { get; set; }
        public string Sexo { get; set; }//TODO: REVER ESSE CAMPO SEXO NA CLASSE POCO
        public string Skype { get; set; }
        public DateTime? DataNascimento { get; set; }
        public DateTime? DataFuncao { get; set; }

        public int IdUsuario { get; set; }
        public int? IdCargo { get; set; }
        public int? IdDepartamento { get; set; }
        
        public int? IdGestor { get; set; }

        public Usuario Usuario { get; set; }
        public Cargo Cargo { get; set; }
        public Departamento Departamento { get; set; }
        public Funcionario Gestor { get; set; }
    }
}

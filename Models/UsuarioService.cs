using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Models
{
    public class UsuarioService
    {

        public void incluirUsuario(Usuario novoUser){
            using(BibliotecaContext bc = new BibliotecaContext()){
                novoUser.Senha = Cryptographya.TextoCryptographado(novoUser.Senha);
                bc.Usuarios.Add(novoUser);
                bc.SaveChanges();
            }
        }
        //listar
        public List<Usuario> Listar(){

            using(BibliotecaContext bc = new BibliotecaContext()){

               return bc.Usuarios.ToList();
                     
            }           
        }

        // buscar por Id
        public Usuario Listar(int id){
            using (BibliotecaContext bc = new BibliotecaContext()){

                return bc.Usuarios.Find(id);
            }
        }

         


        public void editarUsuario(Usuario userEditar){
            using(BibliotecaContext bc = new BibliotecaContext()){

               Usuario u = bc.Usuarios.Find(userEditar.Id);
               u.Nome = userEditar.Nome;
               u.Login =userEditar.Login;
               u.Senha =Cryptographya.TextoCryptographado(userEditar.Senha);
               u.Tipo = userEditar.Tipo;

               bc.SaveChanges(); 
            }

        }

        public void excluirUsuario(int id){

            using(BibliotecaContext bc = new BibliotecaContext()){
                
                Usuario usuarioEncontrado = bc.Usuarios.Find(id);
                bc.Usuarios.Remove(usuarioEncontrado); 
                bc.SaveChanges();
            }
        }


        public Usuario ObterPorId(int id)
        {   
            using(BibliotecaContext bc = new BibliotecaContext())
            {
                return bc.Usuarios.Find(id);  
            }
        }

            public Usuario GetPostDetail(int id)
            {  using (var context = new BibliotecaContext())
            {Usuario registro = context.Usuarios.Where(p => p.Id == id).SingleOrDefault();
            return registro;
            }
            }

        
    }
}
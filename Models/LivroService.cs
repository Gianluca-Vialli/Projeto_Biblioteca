using System.IO.Enumeration;
using System.ComponentModel;
using System.Reflection.Metadata;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections;

namespace Biblioteca.Models
{
    public class LivroService
    {
        public void Inserir(Livro l)
        {
            using (BibliotecaContext bc = new BibliotecaContext())
            {
                bc.Livros.Add (l);
                bc.SaveChanges();
            }
        }

        public void Atualizar(Livro l)
        {
            using (BibliotecaContext bc = new BibliotecaContext())
            {
                Livro livro = bc.Livros.Find(l.Id);
                livro.Autor = l.Autor;
                livro.Titulo = l.Titulo;
                livro.Ano = l.Ano;

                bc.SaveChanges();
            }
        }

        public ICollection<Livro> ListarTodos(FiltrosLivros filtro = null)
        {
            using (BibliotecaContext bc = new BibliotecaContext())
            {
                IQueryable<Livro> query;

                if (filtro != null)
                {
                    //definindo dinamicamente a filtragem
                    switch (filtro.TipoFiltro)
                    {
                        case "Autor":
                            query =
                                bc
                                    .Livros
                                    .Where(l =>
                                        l.Autor.Contains(filtro.Filtro));
                            break;
                        case "Titulo":
                            query =
                                bc
                                    .Livros
                                    .Where(l =>
                                        l.Titulo.Contains(filtro.Filtro));
                            break;
                        default:
                            query = bc.Livros;
                            break;
                    }
                }
                else
                {
                    // caso filtro n??o tenha sido informado
                    query = bc.Livros;
                }

                //ordena????o padr??o
                return query.OrderBy(l => l.Titulo).ToList();
            }
        }

        public ICollection<Livro> ListarDisponiveis()
        {
            using (BibliotecaContext bc = new BibliotecaContext())
            {
                //busca os livros onde o id n??o est?? entre os ids de livro em empr??stimo
                // utiliza uma subconsulta
                return bc.Livros.Where(l =>!(bc.Emprestimos.Where(e => e.Devolvido == false).Select(e => e.LivroId).Contains(l.Id))).ToList();
                
            }
        }

         public ICollection<Livro> GetLivros(string q, string ordem, int page, int size) // esse metodo ?? usado somente por livros e ?? fundamental para a pagina??ao
            {
            using (var context = new BibliotecaContext())
            {
                int pular = (page - 1)* size;

                IQueryable<Livro> consulta = context.Livros.Where(l=>l.Titulo.Contains(q,StringComparison.OrdinalIgnoreCase));
                
                
                if(ordem == "t"){
                    consulta = consulta.OrderBy(l=>l.Titulo);
                }else{
                    if(ordem == "a"){
                    consulta = consulta.OrderBy(l=>l.Autor);
                    }
                }
                

                return consulta.Skip(pular).Take(size).ToList();
            
            }
            
            }
           public int CountRegistro(){
                using(var context = new BibliotecaContext()){
                    return context.Livros.Count();
                }
            }

        public Livro ObterPorId(int id)
        {
            using (BibliotecaContext bc = new BibliotecaContext())
            {
                return bc.Livros.Find(id);
            }
        }
    }
}

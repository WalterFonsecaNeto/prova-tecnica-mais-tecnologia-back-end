using ProvaTecnica.Dominio.Models;
using Newtonsoft.Json;
namespace ProvaTecnica.Dominio.Entidades
{

    public class Pessoa
    {
        #region Propiedades
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public DateOnly DataNascimento { get; set; }
        public string Telefone { get; set; }
        public string Endereco { get; set; }

        #endregion


        #region Funções

        public async Task<List<Pessoa>> DesserializarJsonAsync(string caminhoJson)
        {

            // Verifica se o arquivo existe
            if (!File.Exists(caminhoJson))
            {
                throw new Exception("Arquivo não encontrado");
            }

            // Lê o arquivo JSON e converte para uma lista de Pessoa em string 
            var listaPessoasjson = await File.ReadAllTextAsync(caminhoJson);

            // Converte a lista de strings para uma lista de Pessoa
            var listaPessoasDesserializada = JsonConvert.DeserializeObject<List<Pessoa>>(listaPessoasjson);

            // Verifica se a lista de pessoas foi desserializada com sucesso
            if (listaPessoasDesserializada.Count() == 0)
            {
                throw new Exception("Nenhuma pessoa encontrada na lista de pessoas");
            }

            return listaPessoasDesserializada;

        }

        public async Task SerializarJsonAsync(List<Pessoa> listaPessoas, string caminhoJson)
        {
            // Verifica se o arquivo existe
            if (!File.Exists(caminhoJson))
            {
                throw new Exception("Arquivo não encontrado");
            }

            // Verifica se o arquivo existe
            if (listaPessoas.Count() == 0)
            {
                throw new Exception("Nenhuma pessoa encontrada na lista de pessoas");
            }

            // Converte a lista de Pessoa em uma string JSON
            var jsonSerializado = JsonConvert.SerializeObject(listaPessoas, Formatting.Indented);

            // Salva a string JSON no arquivo JSON
            await File.WriteAllTextAsync(caminhoJson, jsonSerializado);
        }

        public async Task<Pessoa> ObterPessoaPorEmailAsync(string email, string caminhoJson)
        {
            //Desserializa o JSON
            var listaPessoasDesserializadas = await DesserializarJsonAsync(caminhoJson);

            //Obtem a pessoa na lista de pessoas pelo email
            var pessoaEncontrada = listaPessoasDesserializadas.FirstOrDefault(usuario => usuario.Email == email);

            //Verifica se a pessoa foi encontrada na lista de pessoas
            if (pessoaEncontrada == null)
            {
                throw new Exception("Pessoa não encontrada");
            }

            return pessoaEncontrada;

        }

        public async Task<Pessoa> ObterPessoaPorIdAsync(int pessoaId, string caminhoJson)
        {
            //Desserializa o JSON
            var listaPessoasDesserializadas = await DesserializarJsonAsync(caminhoJson);

            //Obtem a pessoa na lista de pessoas pelo id
            var pessoaEncontrada = listaPessoasDesserializadas.FirstOrDefault(usuario => usuario.Id == pessoaId);

            //Verifica se a pessoa foi encontrada na lista de pessoas
            if (pessoaEncontrada == null)
            {
                throw new Exception("Pessoa não encontrada");
            }

            return pessoaEncontrada;

        }

        public async Task AdicionarPessoaAsync(PessoaDTO pessoaDTO, string caminhoJson)
        {
            //verifica se os campos da pessoaDTO sao nulos ou vazios
            VerificarInformacoesPessoaDTO(pessoaDTO);

            //Desserializa o JSON
            var listaPessoas = await DesserializarJsonAsync(caminhoJson);

            //Busca pessoas na lista que tenham o mesmo e-mail para verificar se o e-mail ja existe
            var pessoaEncontrada = listaPessoas.FirstOrDefault(usuario => usuario.Email == pessoaDTO.Email);

            //verifica se o e-mail existe
            if (pessoaEncontrada != null)
            {
                throw new Exception("Email já existe");

            }

            //Busca a pessoa na lista que tenha o maior id
            int maiorUsuarioId = listaPessoas.Max(usuario => usuario.Id);

            //Cria uma pessoa com as informações do DTO e o id para salvar na lista 
            var novaPessoa = new Pessoa()
            {
                Id = maiorUsuarioId + 1,
                Nome = pessoaDTO.Nome,
                Email = pessoaDTO.Email,
                DataNascimento = pessoaDTO.DataNascimento,
                Telefone = pessoaDTO.Telefone,
                Endereco = pessoaDTO.Endereco,
            };

            // Adiciona a nova pessoa na lista de pessoas
            listaPessoas.Add(novaPessoa);

            //Transforma a lista de pessoa atualizada com a nova pessoa em um JSON
            await SerializarJsonAsync(listaPessoas, caminhoJson);
        }

        public async Task AtualizarPessoaAsync(PessoaDTO pessoaDTO, int pessoaId, string caminhoJson)
        {
            //verifica se os campos da pessoaDTO sao nulos ou vazios
            VerificarInformacoesPessoaDTO(pessoaDTO);

            //Desserializa o JSON
            var listaPessoas = await DesserializarJsonAsync(caminhoJson);

            //Busca a pessoa que sera atualizada para verificar se ela existe 
            var pessoaEncontrada = listaPessoas.FirstOrDefault(usuario => usuario.Id == pessoaId);

            //Verifica se a pessoa existe
            if (pessoaEncontrada == null)
            {
                throw new Exception("Pessoa não encontrada");
            }

            //Define os novos valores para a pessoa 
            pessoaEncontrada.Nome = pessoaDTO.Nome;
            pessoaEncontrada.Email = pessoaDTO.Email;
            pessoaEncontrada.DataNascimento = pessoaDTO.DataNascimento;
            pessoaEncontrada.Telefone = pessoaDTO.Telefone;
            pessoaEncontrada.Endereco = pessoaDTO.Endereco;

            //Transforma a lista de pessoa atualizada com a pessoa que foi atualizada em um JSON
            await SerializarJsonAsync(listaPessoas, caminhoJson);

        }

        public async Task RemoverPessoaPorEmailAsync(string email, string caminhoJson)
        {
            //Desserializa o JSON
            var listaPessoas = await DesserializarJsonAsync(caminhoJson);

            //Busca pessoa na lista que tenha o mesmo e-mail para verificar se a pessoa existe
            var pessoaEncontrada = listaPessoas.FirstOrDefault(usuario => usuario.Email == email);

            //verifica se o a pessoa existe
            if (pessoaEncontrada == null)
            {
                throw new Exception("Pessoa não encontrada");

            }

            //Remove a pessoa da lista de pessoas
            listaPessoas.Remove(pessoaEncontrada);

            //Transforma a lista de pessoa atualizada com a pessoa que foi removida em um JSON
            await SerializarJsonAsync(listaPessoas, caminhoJson);
        }
        
        public async Task RemoverPessoaPorIdAsync(int pessoaId, string caminhoJson)
        {
            //Desserializa o JSON
            var listaPessoas = await DesserializarJsonAsync(caminhoJson);

            //Busca pessoa na lista que tenha o mesmo Id para verificar se a pessoa existe
            var pessoaEncontrada = listaPessoas.FirstOrDefault(usuario => usuario.Id == pessoaId);

            //verifica se o a pessoa existe
            if (pessoaEncontrada == null)
            {
                throw new Exception("Pessoa não encontrada");

            }

            //Remove a pessoa da lista de pessoas
            listaPessoas.Remove(pessoaEncontrada);

            //Transforma a lista de pessoa atualizada com a pessoa que foi removida em um JSON
            await SerializarJsonAsync(listaPessoas, caminhoJson);
        }


        #endregion


        #region Uteis

        private void VerificarInformacoesPessoaDTO(PessoaDTO pessoaDTO)
        {

            if (string.IsNullOrEmpty(pessoaDTO.Nome))
            {
                throw new Exception("Nome é obrigatório");
            }
            if (string.IsNullOrEmpty(pessoaDTO.Email))
            {
                throw new Exception("E-mail é obrigatorio");
            }
            if (string.IsNullOrEmpty(pessoaDTO.Telefone))
            {
                throw new Exception("Telefone é obrigatorio");
            }
            if (string.IsNullOrEmpty(pessoaDTO.Endereco))
            {
                throw new Exception("Endereço é obrigatorio");
            }

        }

        #endregion
    }
}
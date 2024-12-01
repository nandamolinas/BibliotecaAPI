import React, { useState } from "react";

// Definindo o tipo para os dados do cliente
interface Cliente {
  clienteId: number;
  nome: string;
  cpf: string;
  dataDeInicio: string;
}

const CadastrarCliente = () => {
  // Estados para armazenar os dados do cliente, mensagem e se o cliente já existe
  const [cliente, setCliente] = useState<Cliente>({
    clienteId: 0,
    nome: "",
    cpf: "",
    dataDeInicio: "",
  });
  const [mensagem, setMensagem] = useState<string>("");

  // Função para verificar se o cliente já existe
  const verificarClienteExistente = async (clienteId: number) => {
    try {
      const response = await fetch(`http://localhost:5200/api/clientes/${clienteId}`);
      if (response.ok) {
        const clienteExistente = await response.json();
        return clienteExistente !== null; // Se o cliente existir, retorna true
      } else {
        return false;
      }
    } catch (error) {
      console.error("Erro ao verificar cliente:", error);
      return false;
    }
  };

  // Função para manipular o envio do formulário
  const cadastrarCliente = async () => {
    const clienteJaExiste = await verificarClienteExistente(cliente.clienteId);

    if (clienteJaExiste) {
      setMensagem("Cliente com esse ID já cadastrado.");
      return; // Não prossegue com o cadastro
    }

    try {
      const response = await fetch("http://localhost:5200/api/clientes/cadastrar", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(cliente),
      });

      if (response.ok) {
        // Se o cadastro for bem-sucedido, exibe uma mensagem de sucesso
        setMensagem("Cliente cadastrado com sucesso!");
      } else {
        // Caso o cadastro não seja bem-sucedido, exibe uma mensagem de erro
        setMensagem("Erro ao cadastrar cliente.");
      }
    } catch (error) {
      setMensagem("Erro na comunicação com o servidor.");
    }
  };

  // Função para lidar com mudanças nos campos do formulário
  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setCliente((prevCliente) => ({
      ...prevCliente,
      [name]: value,
    }));
  };

  return (
    <div>
      <h2>Cadastrar Cliente</h2>

      {/* Formulário de cadastro */}
      <div>
        <label htmlFor="clienteId">ID do Cliente:</label>
        <input
          type="number"
          id="clienteId"
          name="clienteId"
          value={cliente.clienteId}
          onChange={handleChange}
        />
      </div>
      <div>
        <label htmlFor="nome">Nome:</label>
        <input
          type="text"
          id="nome"
          name="nome"
          value={cliente.nome}
          onChange={handleChange}
        />
      </div>
      <div>
        <label htmlFor="cpf">CPF:</label>
        <input
          type="text"
          id="cpf"
          name="cpf"
          value={cliente.cpf}
          onChange={handleChange}
        />
      </div>
      <div>
        <label htmlFor="dataDeInicio">Data de Início:</label>
        <input
          type="date"
          id="dataDeInicio"
          name="dataDeInicio"
          value={cliente.dataDeInicio}
          onChange={handleChange}
        />
      </div>

      {/* Botão para enviar o formulário */}
      <button onClick={cadastrarCliente}>Cadastrar</button>

      {/* Exibindo a mensagem de sucesso ou erro */}
      {mensagem && <p>{mensagem}</p>}
    </div>
  );
};

export default CadastrarCliente;

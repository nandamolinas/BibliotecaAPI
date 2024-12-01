import React, { useEffect, useState } from "react";

// Definindo o tipo para um cliente
interface Cliente {
  clienteId: number;
  nome: string;
  cpf: string;
  dataDeInicio: string;
}

const ListarClientes = () => {
  const [clientes, setClientes] = useState<Cliente[]>([]); // Estado para armazenar a lista de clientes
  const [erro, setErro] = useState<string>(""); // Estado para armazenar erros, se houver

  useEffect(() => {
    // Requisição para pegar a lista de clientes
    fetch("http://localhost:5200/api/clientes/listar")
      .then((response) => {
        if (!response.ok) {
          throw new Error("Erro ao carregar clientes");
        }
        return response.json();
      })
      .then((data) => setClientes(data))
      .catch((error) => setErro(error.message)); // Captura de erro, se houver
  }, []); // UseEffect vazio para rodar uma vez ao carregar o componente

  return (
    <div>
      <h2>Lista de Clientes</h2>
      
      {/* Exibição de erro caso ocorra */}
      {erro && <p style={{ color: "red" }}>{erro}</p>}

      {/* Exibição da lista de clientes */}
      {clientes.length === 0 ? (
        <p>Não há clientes cadastrados.</p>
      ) : (
        <table>
          <thead>
            <tr>
              <th>ID</th>
              <th>Nome</th>
              <th>CPF</th>
              <th>Data de Início</th>
            </tr>
          </thead>
          <tbody>
            {clientes.map((cliente) => (
              <tr key={cliente.clienteId}>
                <td>{cliente.clienteId}</td>
                <td>{cliente.nome}</td>
                <td>{cliente.cpf}</td>
                <td>{cliente.dataDeInicio}</td>
              </tr>
            ))}
          </tbody>
        </table>
      )}
    </div>
  );
};

export default ListarClientes;

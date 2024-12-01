import React, { useState } from "react";

const ExcluirCliente = () => {
 
  const [clienteId, setClienteId] = useState<number>(0);
  const [mensagem, setMensagem] = useState<string>("");

  
  const excluirCliente = async () => {
    try {
      const response = await fetch(`http://localhost:5200/api/clientes/excluir/${clienteId}`, {
        method: "DELETE", 
      });

      if (response.ok) {
    
        setMensagem("Cliente excluído com sucesso!");
      } else {

        setMensagem("Erro ao excluir o cliente.");
      }
    } catch (error) {
      setMensagem("Erro na comunicação com o servidor.");
    }
  };


  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setClienteId(Number(e.target.value)); 
  };

  return (
    <div>
      <h2>Excluir Cliente</h2>

     
      <div>
        <label htmlFor="clienteId">ID do Cliente:</label>
        <input
          type="number"
          id="clienteId"
          value={clienteId}
          onChange={handleChange}
        />
      </div>

      <button onClick={excluirCliente}>Excluir</button>

      
      {mensagem && <p>{mensagem}</p>}
    </div>
  );
};

export default ExcluirCliente;

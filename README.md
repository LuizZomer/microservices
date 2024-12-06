# Documento de Requisitos

## 1. Propósito do Sistema

O sistema permite o gerenciamento de produtos e estoques, incluindo:

- Operações de **cadastro**, **consulta**, **atualização** e **exclusão** de produtos.
- Notificações para **estoque crítico**.
- Garantia de eficiência e integração por meio de **APIs REST**.

---

## 2. Usuários

- **Administradores do Sistema**:  
  Realizam operações de cadastro e manutenção de produtos e estoques.

- **Equipe de Estoque**:  
  Monitora e gerencia o nível de produtos no inventário.

- **Vendedores**:  
  Consultam a disponibilidade de produtos.

---

## 3. Requisitos Funcionais

1. **Gerenciamento de Produtos**:  
   Permitir cadastro, consulta, atualização e exclusão de produtos.

2. **Gestão de Estoque**:  
   - Adicionar ou retirar produtos do estoque.
   - Monitorar níveis de estoque de forma automatizada.

3. **Notificações**:  
   Gerar alertas automáticos quando o estoque atingir níveis críticos.

4. **Integração de Serviços**:  
   Garantir consistência e validação de dados entre os seguintes serviços:  
   - **Serviço de Produtos**: Consulta de detalhes de produtos.  
   - **Serviço de Estoque**: Monitoramento de níveis críticos.  
   - **Serviço de Vendas**: Atualização de estoque após transações.

---

## 4. Fluxo das Integrações

- **Consulta de Produtos (Busca 1)**:  
  O serviço de Estoque consulta os detalhes de um produto no serviço de Produtos para validar informações.

- **Monitoramento de Estoque (Busca 2)**:  
  O serviço de Notificações verifica o estoque e gera alertas para níveis baixos.

- **Atualização de Estoque**:  
  Sempre que ocorre uma alteração no estoque, o sistema dispara uma notificação de alerta, se necessário.

---

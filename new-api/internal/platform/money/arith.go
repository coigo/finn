// Aritmética em centavos para as fórmulas de preço médio dos aportes.
// Mul divide por 100 (centavos² → centavos); Div multiplica por 100.
// Divisão por zero entra em pânico — paridade com DivideByZeroException
// do C# (o handler recupera e responde 500).
package money

func (m Money) Add(o Money) Money { return m + o }

func (m Money) Sub(o Money) Money { return m - o }

func (m Money) Neg() Money { return -m }

func (m Money) Mul(o Money) Money { return Money((int64(m) * int64(o)) / CentPerReal) }

func (m Money) Div(o Money) Money { return Money((int64(m) * CentPerReal) / int64(o)) }

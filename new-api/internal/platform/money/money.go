// Money é dinheiro em centavos (INTEGER no SQLite; jamais REAL).
// O contrato HTTP continua sendo número decimal em reais — igual ao decimal
// que o C# emitia. Serialização usa o mínimo de casas necessárias
// (1250 → 1250, 12.5 → 12.5, 12.55 → 12.55); divergência de dígitos
// à direita (12.50 vs 12.5) é documentada na Fase 5.
package money

import (
	"encoding/json"
	"fmt"
	"math/big"
	"strings"
)

type Money int64

const CentPerReal int64 = 100

func FromCents(c int64) Money { return Money(c) }

func (m Money) Cents() int64 { return int64(m) }

func (m Money) MarshalJSON() ([]byte, error) {
	var b strings.Builder
	if m < 0 {
		b.WriteByte('-')
	}
	abs := m
	if abs < 0 {
		abs = -abs
	}
	cents := abs % Money(CentPerReal)
	reais := abs / Money(CentPerReal)
	b.WriteString(fmt.Sprintf("%d", reais))
	if cents != 0 {
		b.WriteByte('.')
		b.WriteByte(byte('0' + cents/10))
		if cents%10 != 0 {
			b.WriteByte(byte('0' + cents%10))
		}
	}
	return []byte(b.String()), nil
}

func (m *Money) UnmarshalJSON(data []byte) error {
	s := strings.TrimSpace(string(data))
	if len(s) > 1 && s[0] == '"' {
		var str string
		if err := json.Unmarshal(data, &str); err != nil {
			return err
		}
		s = str
	}
	r, ok := new(big.Rat).SetString(s)
	if !ok {
		return fmt.Errorf("valor %q não é decimal válido", s)
	}
	cents := r.Mul(r, big.NewRat(CentPerReal, 1))
	if cents.Denom().Cmp(big.NewInt(1)) != 0 {
		return fmt.Errorf("valor %q tem mais precisão que centavos", s)
	}
	*m = Money(cents.Num().Int64())
	return nil
}

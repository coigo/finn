package money

import "testing"

func TestAritmetica(t *testing.T) {
	if got := FromCents(1000).Add(FromCents(250)); got.Cents() != 1250 {
		t.Errorf("Add = %d", got.Cents())
	}
	if got := FromCents(1000).Sub(FromCents(250)); got.Cents() != 750 {
		t.Errorf("Sub = %d", got.Cents())
	}
	// 10.00 * 100 = 1000.00
	if got := FromCents(1000).Mul(FromCents(10000)); got.Cents() != 100000 {
		t.Errorf("Mul = %d", got.Cents())
	}
	// 1000.00 / 200 = 5.00
	if got := FromCents(100000).Div(FromCents(20000)); got.Cents() != 500 {
		t.Errorf("Div = %d", got.Cents())
	}
	// negativos: 8.00 * (-50) = -400.00
	if got := FromCents(800).Mul(FromCents(-5000)); got.Cents() != -40000 {
		t.Errorf("Mul negativo = %d", got.Cents())
	}
}

func TestDivisaoPorZeroPanica(t *testing.T) {
	defer func() {
		if recover() == nil {
			t.Error("esperava pânico de divisão por zero")
		}
	}()
	FromCents(1000).Div(FromCents(0))
}

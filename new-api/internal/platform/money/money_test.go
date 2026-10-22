package money

import (
	"encoding/json"
	"testing"
)

func TestMarshalJSON(t *testing.T) {
	cases := []struct {
		centavos Money
		esperado string
	}{
		{0, "0"},
		{125000, "1250"},
		{125050, "1250.5"},
		{125055, "1250.55"},
		{-500, "-5"},
	}
	for _, c := range cases {
		b, err := json.Marshal(c.centavos)
		if err != nil {
			t.Fatalf("marshal %d: %v", c.centavos, err)
		}
		if string(b) != c.esperado {
			t.Errorf("marshal %d = %s, esperado %s", c.centavos, b, c.esperado)
		}
	}
}

func TestUnmarshalJSON(t *testing.T) {
	cases := []struct {
		json     string
		esperado Money
	}{
		{"1250", 125000},
		{"12.5", 1250},
		{"12.55", 1255},
		{"-5", -500},
		{"1000", 100000},
	}
	for _, c := range cases {
		var m Money
		if err := json.Unmarshal([]byte(c.json), &m); err != nil {
			t.Fatalf("unmarshal %s: %v", c.json, err)
		}
		if m != c.esperado {
			t.Errorf("unmarshal %s = %d, esperado %d", c.json, m, c.esperado)
		}
	}

	var m Money
	for _, invalido := range []string{"12.555", `"abc"`, "true"} {
		if err := json.Unmarshal([]byte(invalido), &m); err == nil {
			t.Errorf("unmarshal %s deveria falhar", invalido)
		}
	}
}

func TestIdaVolta(t *testing.T) {
	var m Money = 43210
	b, _ := m.MarshalJSON()
	var deVolta Money
	if err := json.Unmarshal(b, &deVolta); err != nil {
		t.Fatal(err)
	}
	if deVolta != m {
		t.Errorf("ida e volta: %d → %s → %d", m, b, deVolta)
	}
}

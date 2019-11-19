public class Animal {
	String genus;
	String species;
	Animal relative;

	public Animal(String g, String s) {
		genus = g;
		species = s;
		relative = null;
	}

	public String getGenus() {
		return genus;
	}

	public String getSpecies() {
		return species;
	}

	public Animal getRelative() {
		return relative;
	}

	public void setRelative(Animal rel) {
		relative = rel;
	}
}

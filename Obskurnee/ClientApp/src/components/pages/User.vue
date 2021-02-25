<template>
<section>
  <div v-if="user">
  <div v-if="mode == 'edit'">
    <div class="profile">
      <div class="form-field">
        <label for="username" class="label">Jméno (povinné):</label>
        <input type="text" class="input" id="username" required v-model="editingUser.name" />
      </div>
      <div class="form-field">
        <label for="userphone" class="label">Telefon:</label>
        <input type="tel" class="input" id="userphone" v-model="editingUser.phone" />
      </div>
      <div class="form-field">
        <label for="usergr" class="label">Profil na Goodreads:</label>
        <input type="url" class="input" id="usergr" v-model="editingUser.goodreadsUrl" />
      </div>
      <div class="form-field">
        <label for="userbio" class="label">O mne:</label> 
        <textarea class="textarea" id="userbio" v-model="editingUser.aboutMe" placeholder="Napis sem nieco o sebe! Ake mas rada zanre? Co su Tvoje oblubene knihy? Co naopak nemas rada? Nieco ine, co nam o sebe povies?

Mozes pouzit Markdown na jednoduche formatovanie textu. 
Ak by si sa stratila, klikni na help ikonku nizsie.
Medzi zaklady patri napriklad: 

# Najvacsi nadpis
## mensi nadpis 

**tucny textt** alebo _kurziva_ 

- necislovany
- zoznam
- je 
- jednoduchy

1. cislovany
2. tiez
3. lahky

> takto sa pise citat
> **moze** obsahovat aj _**formatovanie**_

Mozes lahko pridat aj [link](https://google.sk)"></textarea>
        <div>
          <a href="https://www.markdownguide.org/cheat-sheet/" target="_blank">
            <img src="../../assets/markdown-logo.svg" width="25" height="25"/>
              Pomoc s Markdownom
            </a>
        </div>
      </div>
    </div>
    <div class="profile-button">
      <a @click="updateProfile" class="button-primary button-margin" :v-if="isMod || user.userId == myUserId">Uložit změny</a>
      <a @click="stopEditing" class="button-secondary button-margin" :v-if="isMod || user.userId == myUserId">Zahodit změny</a>
    </div>
  </div>

  <div v-if="mode != 'edit'">
    <div class="profile">
      <div class="profil-row" style="font-size: 2em; font-weight: bold; text-align: center;">
        <h1>{{ user.name }}</h1>
      </div>
      <div class="profile-row">
        <div class="profile-val" v-html="user.aboutMeHtml"></div>
      </div>
      <div class="profile-row">
        <table style="width: 100%">
          <tr>
            <td>
              <div class="profile-val"><a :href="'mailto:' + user.email">📧 {{ user.email }}</a></div>
            </td>
            <td>
              <div class="profile-val">
                <a :href="'tel:' + user.phone">
                  <img src="../../assets/whatsapp-logo.png" width="25" height="25"/>
                    {{ user.phone }}
                  </a>
                </div>
            </td>
            <td>
              <div class="profile -val">
                <a :href="user.goodreadsUrl">
                  <img src="../../assets/goodreads-logo.svg" width="25" height="25"/>
                    Goodreads
                </a>
              </div>
            </td>
          </tr>
        </table>
      </div>
    </div>
    <div class="profile-button">
      <a @click="startEditing" class="button-primary" :v-if="isMod || user.userId == myUserId">Upravit údaje</a>
    </div>
  </div>


  <p class="todo-l"><strong>Laci:</strong>sem co este? Chceme tu naprikald userove reviews, recs?</p>
  <p class="todo"><strong>Rozárka:</strong>To bych nedávala do profilu, ten bych nechala čistě jako přehled osobních údajů určených k editaci. Review a recs bych nechala do sekce My.</p>
  <p class="todo-l"><strong>Laci:</strong>Okej, v tom pripade porozmyslaj nad rozlozenim stranok aj routes. Myslel som, ze budeme mat URL ako mame teraz, ze /my/mailadersa, kde budes vidiet skondenzovane user info a pod tym reviews a take veci. Iba mod by videl moznost 'edit'. <br/> Mozeme to dat aj inam, ale - kam? Ake budu URL?</p>
  <p class="todo"><strong>Rozárka:</strong>Hmm, máš recht. V sekci My by mohly být kartičky s přehledem uživatelů - pro každého třeba jen jméno, profile pic (😁) a bio, případně počet recenzí a recs. A po rozkliknutí by se mohla zobrazit takhle stránka, na které by byly veškeré údaje včetně těch reviews a recs a jen mod/majitel účtu by tam měl tlačítko na editaci.</p>
  <p class="todo"><strong>Rozárka:</strong>Budeme sem přidávat profile pic?</p>
  <p class="todo-l"><strong>Laci:</strong>Dobra otazka, co ja viem? Bude to niekto vyplnat?</p>
  </div>
</section>
</template>

<script>
import { mapActions, mapGetters } from "vuex";

export default {
  name: 'User',
  data() {
      return {
        user: {},
        mode: "default",
        editingUser: {},
      }
  },
  computed: {
    ...mapGetters("context", ["myUserId", "isMod"]),
  },
  methods: {
    ...mapActions("users", ["getUser", "updateUser"]),
    updateProfile() 
    {
      this.updateUser(this.editingUser)
        .then(() => {
          this.fetchProfile();
          this.stopEditing();
        });
    },
    startEditing() 
    {
      this.editingUser = JSON.parse(JSON.stringify(this.user));
      this.mode = "edit";
    },
    stopEditing() 
    {
      this.mode = "default";
    },
    fetchProfile()
    {
      return this.getUser(this.$route.params.email)
        .then(data => this.user = data);
    }
  },
  mounted() {
    this.fetchProfile()
      .then(() =>
      {
        if (this.$route.params.mode == "edit")
        {
          this.startEditing();
        }
      });
  }
}
</script>

<style scoped>

  .profile,
  .profile-button {
    max-width: 800px;
  }

  .profile {
    background-color: var(--c-bckgr-primary);
    margin: var(--spacer);
    padding: calc(2* var(--spacer));
    text-align: left;
  }

  .profile-button {
    margin: 0 var(--spacer);
    text-align: center;
  }

  @media screen and (min-width: 840px) {
    .profile {
      margin: var(--spacer) auto;
    }

    .profile-button {
      margin: 0 auto;
    }
  }

  .profile-row {
    padding: calc(0.5 * var(--spacer)) 0;
  }

  .form-field {
    margin-bottom: var(--spacer);
  }

  .profile-cat,
  .label {
    font-size: 1.2em;
    font-weight: bold;
  }

  .label {
    display: block;
  }

  .input {
    width: 100%;
    border: 0;
    background-color: var(--c-bckgr-secondary);
    padding: 0.5em;
  }

  .input,
  .textarea {
    outline-color: var(--c-accent-secondary);
  }

  .readonly {
    outline: none;
    color: rgba(41, 41, 41, 0.5); /* --c-font plus opacity */
  }

  .textarea {
    font-size: 1em;
    border: 0;
    background-color: var(--c-bckgr-secondary);
    padding: 0.5em 1em;
    width: 100%;
    height: 15em;
  }

  .button-margin {
    margin: calc(0.5 * var(--spacer));
  }

</style>
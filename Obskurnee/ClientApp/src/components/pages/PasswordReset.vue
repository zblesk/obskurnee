<template>
<section>
    <h1 class="page-title">Změna hesla</h1>
    <div class="main">
        <div v-if="$route.params.token">
            <div class="form-field">
                <label for="passwordInput" class="label">Heslo</label>
                <input type="password" class="input" id="passwordInput" @keyup.enter="resetPassword" v-model="newPassword" required />
            </div>
            <button @click="resetPassword" class="button-primary">Reset</button>
        </div>
        <div v-else>
            <div class="form-field">
                <label for="emailInput" class="label">Zadej svůj e-mail</label>
                <input class="input" id="emailInput" @keyup.enter="initPasswordReset" type="email" placeholder="@" v-model="email" required />
            </div>
            <button class="button-primary" @click="initPasswordReset">Odeslat</button>
        </div>
    </div>
</section>
</template>

<style scoped>

    .form-field {
        margin-bottom: var(--spacer);
    }

    .label {
        display: block;
        font-size: 1em;
        margin-bottom: calc(var(--spacer) / 3);
    }

    .input {
        width: 100%;
        max-width: 360px;
        border: 0;
        background-color: var(--c-bckgr-secondary);
        padding: 0.5em;
    }



</style>


<script>
import { mapActions } from "vuex";
export default {
    name: "PasswordReset",
    data() {
        return {
            email: "",
            newPassword: "",
        }
    },
    methods: {
        ...mapActions("context", ["passwordResetFinish", "passwordResetInit", "login"]),
        resetPassword()
        {
            if (!this.newPassword)
            {
                this.$notifyError('Zadaj heslo');
                return;
            }
            this.passwordResetFinish({ 
                password: this.newPassword, 
                userId: this.$route.params.userId, 
                token: this.$route.params.token})
                .then(() => {
                    this.$notifySuccess("Zmenené");
                    this.$router.push({ name: 'home' });
                })
                .catch((err, p) =>{ 
                    console.log('aaaaaaaaaaaa koniec', err, p);
                    console.log(err); 
                    this.$notifyError(err?.response?.data); });
        },
        initPasswordReset() 
        {
            if (!this.email)
            {
                this.$notifyError('Zadaj email');
                return;
            }
            this.passwordResetInit(this.email)
                .then(() => this.$notifySuccess('Pozri mail'))
                .catch((err) => this.$notifyError(err));
        },
    },
    mounted() {
    }
}
</script>

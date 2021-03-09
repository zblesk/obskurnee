<template>
<section>
    <div v-if="$route.params.token">
        <label for="passwordInput">Heslo</label>
        <input id="passwordInput" @keyup.enter="resetPassword" v-model="newPassword" required />
        <button @click="resetPassword">Reset</button>
    </div>
    <div v-else>
        <div class="login-form-field">
            <label for="emailInput">E-mail</label>
            <input id="emailInput" @keyup.enter="initPasswordReset" type="email" v-model="email" required />
        </div>
        <button class="button-reset" @click="initPasswordReset">Odošli</button>
    </div>
</section>
</template>


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

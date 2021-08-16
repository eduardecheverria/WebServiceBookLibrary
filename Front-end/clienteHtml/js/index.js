function guardarLocalStorage(event){
    const user=document.getElementById("user").value;
    const pass=document.getElementById("pass").value;
    localStorage.setItem('user', user);
    localStorage.setItem('pass', pass);
    location.replace("setUser.html");
    event.preventDefault();
}